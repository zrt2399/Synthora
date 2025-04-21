using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using Bogus;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Employee> _employees;

        public MainWindowViewModel()
        {
            var employeeFaker = new Faker<Employee>("zh_CN")
            .RuleFor(e => e.Id, f => f.IndexGlobal + 1)
            .RuleFor(e => e.Name, f => f.Name.LastName() + f.Name.FirstName()) // 中文名

            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(f.Random.AlphaNumeric(8)))
            .RuleFor(e => e.Age, f => f.Random.Int(18, 65))
            .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now)) // 过去10年内的日期
            .RuleFor(e => e.Salary, f => Math.Round(f.Random.Decimal(5000, 50000), 2)); // 薪资范围

            //.FinishWith((f, e) =>
            //{
            //    e.Email = e.Email.ToLower();
            //});

            List<Employee> employees = employeeFaker.Generate(1000);
            _employees = new ObservableCollection<Employee>(employees);
            DataGridCollectionView = new DataGridCollectionView(_employees);
            DataGridCollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(Employee.Age)));
        }

        public string Greeting { get; set; } = "Welcome to Avalonia!";
        public DataGridCollectionView DataGridCollectionView { get; set; }

        public async Task Show()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Caption", "Are you sure you would like to delete appender_replace_page_1?",
               ButtonEnum.YesNo, Icon.Info);

            var result = await box.ShowAsPopupAsync(App.MainWindow);
        }
    }
}