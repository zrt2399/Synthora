using System;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class CollectionControlViewModel : TreeMenuDemoItem
    {
        public CollectionControlViewModel()
        {
            IconKind = MaterialIconKind.FolderMultiple;
        }
    }

    public partial class DataGridViewModel : TreeMenuDemoItem
    {
        public DataGridViewModel()
        {
            IconKind = MaterialIconKind.TableLarge;
            Description = "DataGrid";
            ResetEmployees();
        }

        [ObservableProperty]
        public partial ObservableCollection<Employee>? Employees { get; set; }

        [ObservableProperty]
        public partial DataGridCollectionView? DataGridCollectionView { get; set; }

        public void ClearEmployees()
        {
            Employees?.Clear();
        }

        public void ResetEmployees()
        {
            //Comment below to enable Native AOT compilation
            var employeeFaker = new Faker<Employee>("zh_CN")
                .RuleFor(e => e.Id, f => f.IndexGlobal + 1)
                .RuleFor(e => e.Name, f => f.Name.LastName() + f.Name.FirstName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(f.Random.AlphaNumeric(8)))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 65))
                .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Now))
                .RuleFor(e => e.Salary, f => f.Random.Int(5000, 50000))
                .RuleFor(e => e.IsActive, f => f.Random.Bool());
            var employees = employeeFaker.Generate(1000);


            Employees = new ObservableCollection<Employee>(employees);
            DataGridCollectionView = new DataGridCollectionView(Employees);
            DataGridCollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(Employee.Age)));
        }
    }

    public class ListBoxViewModel : TreeMenuDemoItem
    {
        public ListBoxViewModel()
        {
            IconKind = MaterialIconKind.ListBox;
            Description = "ListBox, ListBoxItem";
        }
    }

    public class TreeViewViewModel : TreeMenuDemoItem
    {
        public TreeViewViewModel()
        {
            IconKind = MaterialIconKind.FileTree;
            Description = "TreeView, TreeViewItem";
        }
    }
}