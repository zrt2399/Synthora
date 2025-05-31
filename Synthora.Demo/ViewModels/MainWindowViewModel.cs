using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.Threading;
using Bogus;
using Synthora.Demo.Models;
using Synthora.Messaging;

namespace Synthora.Demo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private WindowNotificationManager? _notificationManager;

        public MainWindowViewModel()
        {
            var employeeFaker = new Faker<Employee>("zh_CN")
                .RuleFor(e => e.Id, f => f.IndexGlobal + 1)
                .RuleFor(e => e.Name, f => f.Name.LastName() + f.Name.FirstName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(f.Random.AlphaNumeric(8)))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 65))
                .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now))
                .RuleFor(e => e.Salary, f => Math.Round(f.Random.Decimal(5000, 50000), 2))
                .RuleFor(e => e.IsActive, f => f.Random.Bool());

            List<Employee> employees = employeeFaker.Generate(1000);
            Employees = new ObservableCollection<Employee>(employees);
            DataGridCollectionView = new DataGridCollectionView(Employees);
            DataGridCollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(Employee.Age)));
            Dispatcher.UIThread.InvokeAsync(() => _notificationManager = new WindowNotificationManager(App.MainWindow));
        }

        public ObservableCollection<Employee> Employees { get; }

        public string Greeting { get; set; } = "Welcome to Avalonia!";
        public double Value { get; set; }
        public IList? SelectedFiles { get; set; }
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

        public DataGridCollectionView DataGridCollectionView { get; }

        public ObservableCollection<string> BrushKeys { get; } = new ObservableCollection<string>
        {
            "PrimaryBrush",
            "SuccessBrush",
            "WarningBrush",
            "DangerBrush",
            "ErrorBrush",
            "ThemeAccentBrush",
            "ThemeAccentBrush2",
            "ThemeAccentBrush3",
            "ThemeAccentBrush4",
            "ThemeBackgroundBrush",
            "ThemeForegroundBrush"
        };

        public void ClearEmployees()
        {
            Employees.Clear();
        }

        public void Test() => Greeting = "Test";

        public async void ShowAlertDialog(string param)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < 50; i++)
            {
                stringBuilder.Append($"{param} Message;");
            }
            var message = stringBuilder.ToString();

            var result = param switch
            {
                nameof(IconType.Information) => await AlertDialog.ShowAsPopupAsync(App.MainWindow, message, "Information", DialogButton.OK, IconType.Information),
                nameof(IconType.Question) => await AlertDialog.ShowAsPopupAsync(App.MainWindow, message, "Question", DialogButton.OK | DialogButton.Cancel, IconType.Question),
                nameof(IconType.Warning) => await AlertDialog.ShowAsPopupAsync(App.MainWindow, message, "Warning", DialogButton.Yes | DialogButton.No, IconType.Warning),
                nameof(IconType.Error) => await AlertDialog.ShowAsPopupAsync(App.MainWindow, message, "Error", DialogButton.Yes | DialogButton.No | DialogButton.Cancel, IconType.Error),
                _ => await AlertDialog.ShowAsPopupAsync(App.MainWindow, message, "Abort", DialogButton.Yes | DialogButton.No | DialogButton.Cancel | DialogButton.Abort, IconType.Error)
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
            switch (param)
            {
                case nameof(MessageTip.Show):
                    var stringBuilder = new StringBuilder();
                    for (int i = 0; i < 100; i++)
                    {
                        stringBuilder.Append("MessageTip.ShowSuccess;");
                    }
                    MessageTip.Show(stringBuilder.ToString());
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

            switch (param)
            {
                case nameof(NotificationType.Information):
                    _notificationManager.Show(new Notification("Information", "Save file locally completed.", NotificationType.Information));
                    break;
                case nameof(NotificationType.Success):
                    _notificationManager.Show(new Notification("Success", "The file was saved locally.", NotificationType.Success));
                    break;
                case nameof(NotificationType.Warning):
                    _notificationManager.Show(new Notification("Warning", "Failed to save file locally.", NotificationType.Warning));
                    break;
                case nameof(NotificationType.Error):
                    _notificationManager.Show(new Notification("Error", "Error saving file locally.", NotificationType.Error));
                    break;
            }
        }
    }
}