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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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
                .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now)) // 过去10年内的日期
                .RuleFor(e => e.Salary, f => Math.Round(f.Random.Decimal(5000, 50000), 2)) // 薪资范围
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

        public async void ShowMessageBox(string param)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Caption", "Are you sure you would like to delete appender_replace_page_1?",
                ButtonEnum.YesNo, Icon.Info);

            var result = param switch
            {
                nameof(box.ShowAsPopupAsync) => await box.ShowAsPopupAsync(App.MainWindow),
                nameof(box.ShowWindowDialogAsync) => await box.ShowWindowDialogAsync(App.MainWindow),
                nameof(box.ShowWindowAsync) => await box.ShowWindowAsync(),
                _ => await box.ShowAsync()
            };
            await new Window()
            {
                Content = result,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                CanResize = false,
                Height = 180,
                Width = 320,
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
                    _notificationManager.Show(new Notification("操作完成", "文件保存至本地完成。", NotificationType.Information));
                    break;
                case nameof(NotificationType.Success):
                    _notificationManager.Show(new Notification("操作成功", "文件已保存至本地。", NotificationType.Success));
                    break;
                case nameof(NotificationType.Warning):
                    _notificationManager.Show(new Notification("操作失败", "文件保存至本地失败。", NotificationType.Warning));
                    break;
                case nameof(NotificationType.Error):
                    _notificationManager.Show(new Notification("操作异常", "文件保存至本地异常。", NotificationType.Error));
                    break;
            }
        }
    }
}