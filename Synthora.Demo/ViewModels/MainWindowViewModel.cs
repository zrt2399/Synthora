using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.Threading;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Synthora.Demo.Models;
using Synthora.Messaging;

namespace Synthora.Demo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private WindowNotificationManager? _notificationManager;

        public MainWindowViewModel()
        {
            //Comment below to enable Native AOT compilation
            var employeeFaker = new Faker<Employee>("zh_CN")
                .RuleFor(e => e.Id, f => f.IndexGlobal + 1)
                .RuleFor(e => e.Name, f => f.Name.LastName() + f.Name.FirstName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(f.Random.AlphaNumeric(8)))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 65))
                .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now))
                .RuleFor(e => e.Salary, f => Math.Round(f.Random.Decimal(5000, 50000), 2))
                .RuleFor(e => e.IsActive, f => f.Random.Bool());
            var employees = employeeFaker.Generate(1000);


            Employees = new ObservableCollection<Employee>(employees);
            DataGridCollectionView = new DataGridCollectionView(Employees);
            DataGridCollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(Employee.Age)));
            Dispatcher.UIThread.InvokeAsync(() => _notificationManager = new WindowNotificationManager(App.MainWindow));
            if (Application.Current is Application application)
            {
                application.ActualThemeVariantChanged += Current_ActualThemeVariantChanged;
            }
            InitializeBrushResources();
        }

        [ObservableProperty]
        public partial ObservableCollection<Employee> Employees { get; set; }
        [ObservableProperty]
        public partial DataGridCollectionView DataGridCollectionView { get; set; }

        [ObservableProperty]
        public partial string Greeting { get; set; } = "Welcome to Avalonia!";
        [ObservableProperty]
        public partial NotificationPosition NotificationPosition { get; set; } = NotificationPosition.TopRight;
        public IEnumerable<NotificationPosition> NotificationPositions { get; } = Enum.GetValues<NotificationPosition>();
        [ObservableProperty]
        public partial PlacementMode MessageTipPlacement { get; set; } = PlacementMode.Pointer;
        public IEnumerable<PlacementMode> MessageTipPlacements { get; } = Enum.GetValues<PlacementMode>();
        [ObservableProperty]
        public partial DialogButton DialogButton { get; set; } = DialogButton.OK;
        public IEnumerable<DialogButton> DialogButtons { get; } = Enum.GetValues<DialogButton>();
        [ObservableProperty]
        public partial double Value { get; set; }
        [ObservableProperty]
        public partial IList? SelectedFiles { get; set; }

        public IReadOnlyList<FilePickerFileType> FilePickerFileTypes { get; } = new List<FilePickerFileType>
        {
            new FilePickerFileType("Image Files")
            {
                Patterns = ["*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp"]
            },
            new FilePickerFileType("Text Files")
            {
                Patterns = ["*.txt"]
            },
            new FilePickerFileType("All Files")
            {
                Patterns = ["*"]
            }
        };

        [ObservableProperty]
        public partial ObservableCollection<string>? BrushKeys { get; set; }

        public static Uri GitHubDepositoryUri { get; } = new Uri("https://github.com/zrt2399/Synthora");

        private void InitializeBrushResources()
        {
            if (Application.Current is not Application application
                || application.Styles.FirstOrDefault(x => x is SynthoraTheme) is not SynthoraTheme synthoraTheme)
            {
                return;
            }

            List<string> keys = [];
            keys.AddRange(synthoraTheme.Resources.Select(x => x.Key.ToString()!).Where(IsBrush));
            if (synthoraTheme.Resources.ThemeDictionaries[ThemeVariant.Default] is ResourceDictionary dictionary)
            {
                keys.AddRange(dictionary.Keys.Select(x => x.ToString()!).Where(IsBrush));
            }

            BrushKeys = new ObservableCollection<string>(keys);
        }

        private static bool IsBrush(string? key)
        {
            return !string.IsNullOrEmpty(key) && (key.Contains("Brush") || key.Contains("Background") || key.Contains("Foreground")) && !key.Contains("Color");
        }

        private void Current_ActualThemeVariantChanged(object? sender, EventArgs e)
        {
            var temp = BrushKeys;
            if (temp != null)
            {
                BrushKeys = new ObservableCollection<string>(temp);
            }
        }

        public void ClearEmployees()
        {
            Employees.Clear();
        }

        public void Test() => Greeting = "Test";

        public async void ShowAlertDialog(string param)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                stringBuilder.Append($"{param} Message;");
            }
            var message = stringBuilder.ToString();

            var result = param switch
            {
                nameof(IconType.Information) => await AlertDialog.ShowAsync("AlertDialogHost", message, "Information", DialogButton, IconType.Information),
                nameof(IconType.Question) => await AlertDialog.ShowAsync(message, "Question", DialogButton, IconType.Question),
                nameof(IconType.Warning) => await AlertDialog.ShowAsync(message, "Warning", DialogButton, IconType.Warning),
                nameof(IconType.Error) => await AlertDialog.ShowAsync(message, "Error", DialogButton, IconType.Error),
                _ => await AlertDialog.ShowAsync(message, "Abort", DialogButton | DialogButton.Abort, IconType.Error)
            };
            await new Window()
            {
                Content = $"Selected:{result}",
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

        public void ShowMessageTip(string param)
        {
            MessageTip.Placement = MessageTipPlacement;
            switch (param)
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
            }
        }

        public void SwitchTheme(string param)
        {
            if (Application.Current is Application application)
            {
                application.RequestedThemeVariant = param switch
                {
                    nameof(ThemeVariant.Default) => ThemeVariant.Default,
                    nameof(ThemeVariant.Light) => ThemeVariant.Light,
                    nameof(ThemeVariant.Dark) => ThemeVariant.Dark,
                    _ => application.RequestedThemeVariant
                };
            }
        }

        public void ShowNotification(string param)
        {
            if (_notificationManager == null)
            {
                return;
            }

            _notificationManager.Position = NotificationPosition;
            switch (param)
            {
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
            }
        }

        public static ICommand ShowMainWindowCommand { get; } = new RelayCommand(() =>
        {
            if (!App.MainWindow.IsVisible)
            {
                App.MainWindow.Show();
            }
            if (App.MainWindow.WindowState == WindowState.Minimized)
            {
                App.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                App.MainWindow.Activate();
            }
        });

        public static ICommand OpenGitHubCommand { get; } = new RelayCommand(() => Process.Start(new ProcessStartInfo
        {
            FileName = GitHubDepositoryUri.ToString(),
            UseShellExecute = true
        }));
    }
}