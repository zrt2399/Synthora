using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Layout;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;
using Synthora.Overlays;

namespace Synthora.Demo.ViewModels
{
    public partial class OverlayViewModel : TreeMenuDemoItem
    {
        private WindowNotificationManager? _notificationManager;

        [ObservableProperty]
        public partial NotificationPosition NotificationPosition { get; set; } = NotificationPosition.TopRight;

        [ObservableProperty]
        public partial PlacementMode MessageTipPlacement { get; set; } = PlacementMode.Pointer;

        [ObservableProperty]
        public partial DialogButton DialogButton { get; set; } = DialogButton.OK;

        public OverlayViewModel()
        {
            IconKind = MaterialIconKind.Bell;
            Description = "Notification, MessageTip, AlertDialog";
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _notificationManager = new WindowNotificationManager(App.MainWindow);
            });
        }

        public async void ShowAlertDialog(object? content)
        {
            var type = content?.ToString();
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                stringBuilder.Append($"{type} Message;");
            }
            var message = stringBuilder.ToString();

            var result = type switch
            {
                nameof(IconType.None) => await AlertDialog.ShowAsync("AlertDialogHost", message, type, DialogButton, IconType.None),
                nameof(IconType.Information) => await AlertDialog.ShowAsync(message, type, DialogButton, IconType.Information),
                nameof(IconType.Question) => await AlertDialog.ShowAsync(message, type, DialogButton, IconType.Question),
                nameof(IconType.Success) => await AlertDialog.ShowAsync(message, type, DialogButton, IconType.Success),
                nameof(IconType.Warning) => await AlertDialog.ShowAsync(message, type, DialogButton, IconType.Warning),
                nameof(IconType.Error) => await AlertDialog.ShowAsync(message, type, DialogButton, IconType.Error),
                "Abort" => await AlertDialog.ShowAsync(message, type, DialogButton | DialogButton.Abort, IconType.Error),
                _ => await AlertDialog.ShowAsync(new AlertDialogOptions()
                {
                    DialogButton = DialogButton,
                    Title = type,
                    Message = message,
                    IconType = IconType.Information,
                    IsFooterStretched = true,
                    VerticalAlignment = VerticalAlignment.Top
                }),
            };
            await new Window()
            {
                Content = $"Selected: {result}",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                CanResize = false,
                Height = 140,
                Width = 280,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }.ShowDialog(App.MainWindow);
            //https://github.com/AvaloniaUI/Avalonia/issues/15649
            //The modal dialog flashes on macOS when clicking on the parent window
        }

        public void ShowMessageTip(object? content)
        {
            var type = content?.ToString();
            MessageTip.Placement = MessageTipPlacement;
            switch (type)
            {
                case nameof(MessageTip.Show):
                    MessageTip.Show("Show");
                    break;
                case nameof(MessageTip.ShowSuccess):
                    MessageTip.ShowSuccess("ShowSuccess");
                    break;
                case nameof(MessageTip.ShowQuestion):
                    MessageTip.ShowQuestion("ShowQuestion");
                    break;
                case nameof(MessageTip.ShowWarning):
                    MessageTip.ShowWarning("ShowWarning");
                    break;
                case nameof(MessageTip.ShowError):
                    MessageTip.ShowError("ShowError");
                    break;
                case nameof(IconType.None):
                    MessageTip.Show("None", IconType.None);
                    break;
            }
        }

        public void ShowNotification(object? content)
        {
            if (_notificationManager == null)
            {
                return;
            }

            var type = content?.ToString();
            _notificationManager.Position = NotificationPosition;
            switch (type)
            {
                case "WithoutTitle":
                    _notificationManager.Show(new Notification(null, "This is a without title message.", NotificationType.Information));
                    break;
                case nameof(NotificationType.Information):
                    _notificationManager.Show(new Notification("Information", "This is an information message.", NotificationType.Information));
                    break;
                case nameof(NotificationType.Success):
                    _notificationManager.Show(new Notification("Success", "This is a success message.", NotificationType.Success));
                    break;
                case nameof(NotificationType.Warning):
                    _notificationManager.Show(new Notification("Warning", "This is a warning message.", NotificationType.Warning));
                    break;
                case nameof(NotificationType.Error):
                    _notificationManager.Show(new Notification("Error", "This is an error message.", NotificationType.Error));
                    break;
                case "Multiline":
                    var stringBuilder = new StringBuilder();
                    for (int i = 0; i < 100; i++)
                    {
                        stringBuilder.Append("Multiline;");
                    }
                    var message = stringBuilder.ToString();
                    _notificationManager.Show(new Notification(message, message, NotificationType.Error));
                    break;
            }
        }
    }
}