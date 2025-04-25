using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Platform.Storage;
using Bogus;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Synthora.Demo.Models;
using Synthora.Messaging;

namespace Synthora.Demo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            var employeeFaker = new Faker<Employee>("zh_CN")
                .RuleFor(e => e.Id, f => f.IndexGlobal + 1)
                .RuleFor(e => e.Name, f => f.Name.LastName() + f.Name.FirstName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(f.Random.AlphaNumeric(8)))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 65))
                .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now)) // 过去10年内的日期
                .RuleFor(e => e.Salary, f => Math.Round(f.Random.Decimal(5000, 50000), 2)); // 薪资范围

            List<Employee> employees = employeeFaker.Generate(1000);
            Employees = new ObservableCollection<Employee>(employees);
            DataGridCollectionView = new DataGridCollectionView(Employees);
            DataGridCollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(Employee.Age)));
        }

        public ObservableCollection<Employee> Employees { get; }

        public string Greeting { get; set; } = "Welcome to Avalonia!";
        public double Value { get; set; }
        public IList? SelectedFiles { get; set; }
        public IReadOnlyList<FilePickerFileType> FilePickerFileTypes { get; } = new List<FilePickerFileType>
        {
            new FilePickerFileType("Image Files"){ Patterns = new [] { "*.jpg", "*.jpeg", "*.png", "*.gif" }},
            new FilePickerFileType("Text Files"){ Patterns = new [] { "*.txt" }},
            new FilePickerFileType("All Files"){ Patterns = new [] { "*" }}
        };

        public DataGridCollectionView DataGridCollectionView { get; }

        public async void ShowMessageBox()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Caption", "Are you sure you would like to delete appender_replace_page_1?",
               ButtonEnum.YesNo, Icon.Info);

            var result = await box.ShowAsPopupAsync(App.MainWindow);
        }

        public void ShowMessageTip(string param)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                stringBuilder.Append("MessageTip.ShowOK;");
            }
            if (param == "Show")
            {
                MessageTip.Show(stringBuilder.ToString());
            }
            else if (param == "OK")
            {
                MessageTip.ShowOK("ShowOK");
            }
            else if (param == "Warning")
            {
                MessageTip.ShowWarning("ShowWarning");
            }
            else if (param == "Error")
            {
                MessageTip.ShowError("ShowError");
            }
        }
    }
}