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

        [ObservableProperty]
        public partial bool? IsAllExpanded { get; set; }

        public void ExpandAllGroups()
        {
            IsAllExpanded = null;
            IsAllExpanded = true;
        }

        public void CollapseAllGroups()
        {
            IsAllExpanded = null;
            IsAllExpanded = false;
        }

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
    public partial class TableViewViewModel : TreeMenuDemoItem
    {
        public TableViewViewModel()
        {
            IconKind = MaterialIconKind.Table;
            Description = "TableView";
            var employees = EmployeeGenerator.Generate(1000);
            Employees = new ObservableCollection<Employee>(employees);
        }

        [ObservableProperty]
        public partial ObservableCollection<Employee>? Employees { get; set; }
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

    public class RefreshContainerViewModel : TreeMenuDemoItem
    {
        public RefreshContainerViewModel()
        {
            IconKind = MaterialIconKind.Refresh;
            Description = "RefreshContainer, RefreshVisualizer";
        }
    }
}