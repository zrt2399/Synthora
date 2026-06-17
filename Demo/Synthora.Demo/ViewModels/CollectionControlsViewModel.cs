using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;
using Synthora.Demo.SampleData;

namespace Synthora.Demo.ViewModels
{
    public class CollectionControlsViewModel : TreeMenuDemoItem
    {
        public CollectionControlsViewModel()
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
            var employees = EmployeeGenerator.Generate(1000);
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