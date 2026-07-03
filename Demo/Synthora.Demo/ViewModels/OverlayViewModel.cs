using System.Text;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Layout;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using Synthora.Controls;
using Synthora.Demo.Models;
using Synthora.Overlays;

namespace Synthora.Demo.ViewModels
{
    public partial class OverlayViewModel : TreeMenuDemoItem
    {
        public const string DialogIdentifier = "GlobalDialogHost";

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
            Description = "Notification, MessageTip, AlertDialog, DialogHost";
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _notificationManager = new WindowNotificationManager(App.MainWindow);
            });
        }

        public async void ShowGlobalDialog()
        {
            var dialog = new DialogHostSampleDialog(
                "Publish settings",
                "Apply these changes to the live preview? You can keep editing after publishing.",
                DialogIdentifier);

            var result = await DialogHost.Show(dialog, DialogIdentifier);
            MessageTip.Show($"Dialog closed: {result}");
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
                nameof(IconType.None) => await AlertDialog.Show(message, null, DialogButton, IconType.None),
                nameof(IconType.Information) => await AlertDialog.Show(message, type, DialogButton, IconType.Information),
                nameof(IconType.Question) => await AlertDialog.Show(message, type, DialogButton, IconType.Question),
                nameof(IconType.Success) => await AlertDialog.Show(message, type, DialogButton, IconType.Success),
                nameof(IconType.Warning) => await AlertDialog.Show(message, type, DialogButton, IconType.Warning),
                nameof(IconType.Error) => await AlertDialog.Show(message, type, DialogButton, IconType.Error),
                "Abort" => await AlertDialog.Show(message, type, DialogButton | DialogButton.Abort, IconType.Error),
                _ => await AlertDialog.Show(new AlertDialogOptions()
                {
                    DialogButton = DialogButton,
                    Title = type,
                    Message = message,
                    IconType = IconType.Information,
                    IsFooterStretched = true,
                    VerticalAlignment = VerticalAlignment.Top
                }),
            };
            if (App.MainWindow is Window window)
            {
                await new Window()
                {
                    Content = $"Selected: {result}",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    CanResize = false,
                    CanMinimize = false,
                    Height = 140,
                    Width = 280,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }.ShowDialog(window);
                //https://github.com/AvaloniaUI/Avalonia/issues/15649
                //The modal dialog flashes on macOS when clicking on the parent window
            }
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

    public class DialogHostSampleDialog
    {
        private readonly string _hostIdentifier;

        public DialogHostSampleDialog(string title, string message, string hostIdentifier)
        {
            Title = title;
            Message = message;
            _hostIdentifier = hostIdentifier;
            CloseCommand = new RelayCommand<object?>(Close);
        }

        public string Title { get; }

        public string Message { get; }

        public ICommand CloseCommand { get; }

        private void Close(object? parameter)
        {
            DialogHost.Close(parameter, _hostIdentifier, this);
        }
    }
}