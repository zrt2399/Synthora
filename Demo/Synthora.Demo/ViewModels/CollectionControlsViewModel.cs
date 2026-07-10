using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    }

    public partial class TableViewViewModel : TreeMenuDemoItem
    {
        private ObservableCollection<Employee>? _employees;

        public TableViewViewModel()
        {
            IconKind = MaterialIconKind.Table;
            Description = "TableView, Pagination";
            ResetEmployees();
        }

        [ObservableProperty]
        public partial ObservableCollection<Employee>? PagedEmployees { get; set; }

        [ObservableProperty]
        public partial int PageIndex { get; set; }

        [ObservableProperty]
        public partial int PageSize { get; set; } = 50;

        [ObservableProperty]
        public partial int TotalItemCount { get; set; }

        partial void OnPageIndexChanged(int value) => UpdatePagedEmployees();

        partial void OnPageSizeChanged(int value) => UpdatePagedEmployees();

        public void ClearEmployees()
        {
            _employees?.Clear();
            ResetPageIndexOrUpdate();
        }

        public void ResetEmployees()
        {
            var employees = EmployeeGenerator.Generate(1000);
            _employees = new ObservableCollection<Employee>(employees);
            ResetPageIndexOrUpdate();
        }

        private void ResetPageIndexOrUpdate()
        {
            if (PageIndex == 0)
            {
                UpdatePagedEmployees();
                return;
            }

            PageIndex = 0;
        }

        private void UpdatePagedEmployees()
        {
            var employees = _employees;
            TotalItemCount = employees?.Count ?? 0;
            if (employees == null || employees.Count == 0 || PageSize <= 0)
            {
                PagedEmployees = new ObservableCollection<Employee>();
                return;
            }

            var pageCount = (employees.Count - 1) / PageSize + 1;
            var normalizedPageIndex = Math.Clamp(PageIndex, 0, pageCount - 1);
            if (normalizedPageIndex != PageIndex)
            {
                PageIndex = normalizedPageIndex;
                return;
            }

            PagedEmployees = new ObservableCollection<Employee>(
                employees.Skip(PageIndex * PageSize).Take(PageSize));
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

    public class RefreshContainerViewModel : TreeMenuDemoItem
    {
        public RefreshContainerViewModel()
        {
            IconKind = MaterialIconKind.Refresh;
            Description = "RefreshContainer, RefreshVisualizer";
        }
    }
}