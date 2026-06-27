using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;

namespace Synthora.Attaches
{
    public class DataGridAttach
    {
        private static readonly ConditionalWeakTable<object, WeakReference<DataGrid>> CollectionToDataGridMap = [];

        static DataGridAttach()
        {
            AutoScrollToEndProperty.Changed.AddClassHandler<DataGrid, bool>((s, e) => OnAutoScrollToEndChanged(e));
            SelectedItemsAttachProperty.Changed.AddClassHandler<DataGrid, bool>((s, e) => SelectedItemsAttachChanged(e));
            ScrollIntoItemProperty.Changed.AddClassHandler<DataGrid, object?>((s, e) => OnScrollIntoItemChanged(e));
            IsAllExpandedProperty.Changed.AddClassHandler<DataGrid, bool?>((s, e) => OnIsAllExpandedChanged(e));
        }

        public static readonly AttachedProperty<IList?> SelectedItemsProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, IList?>("SelectedItems", defaultBindingMode: BindingMode.TwoWay);

        public static readonly AttachedProperty<bool> SelectedItemsAttachProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, bool>("SelectedItemsAttach");

        public static readonly AttachedProperty<object?> ScrollIntoItemProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, object?>("ScrollIntoItem");

        public static readonly AttachedProperty<bool> AutoScrollToEndProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, bool>("AutoScrollToEnd");

        public static readonly AttachedProperty<bool> IgnoreAutoScrollOnPointerOverProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, bool>("IgnoreAutoScrollOnPointerOver");

        public static readonly AttachedProperty<bool?> IsAllExpandedProperty =
            AvaloniaProperty.RegisterAttached<DataGridAttach, DataGrid, bool?>("IsAllExpanded");

        public static IList? GetSelectedItems(DataGrid obj) => obj.GetValue(SelectedItemsProperty);
        public static void SetSelectedItems(DataGrid obj, IList? value) => obj.SetValue(SelectedItemsProperty, value);

        public static bool GetSelectedItemsAttach(DataGrid obj) => obj.GetValue(SelectedItemsAttachProperty);
        public static void SetSelectedItemsAttach(DataGrid obj, bool value) => obj.SetValue(SelectedItemsAttachProperty, value);

        public static object? GetScrollIntoItem(DataGrid obj) => obj.GetValue(ScrollIntoItemProperty);
        public static void SetScrollIntoItem(DataGrid obj, object? value) => obj.SetValue(ScrollIntoItemProperty, value);

        public static bool GetAutoScrollToEnd(DataGrid obj) => obj.GetValue(AutoScrollToEndProperty);
        public static void SetAutoScrollToEnd(DataGrid obj, bool value) => obj.SetValue(AutoScrollToEndProperty, value);

        public static bool GetIgnoreAutoScrollOnPointerOver(DataGrid obj) => obj.GetValue(IgnoreAutoScrollOnPointerOverProperty);
        public static void SetIgnoreAutoScrollOnPointerOver(DataGrid obj, bool value) => obj.SetValue(IgnoreAutoScrollOnPointerOverProperty, value);

        public static bool? GetIsAllExpanded(DataGrid obj) => obj.GetValue(IsAllExpandedProperty);
        public static void SetIsAllExpanded(DataGrid obj, bool? value) => obj.SetValue(IsAllExpandedProperty, value);

        private static void OnScrollIntoItemChanged(AvaloniaPropertyChangedEventArgs<object?> e)
        {
            if (e.Sender is DataGrid dataGrid && e.NewValue.Value != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    dataGrid.ScrollIntoView(e.NewValue.Value, null);
                }, DispatcherPriority.Loaded);
            }
        }

        private static void SelectedItemsAttachChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
                if (e.NewValue.Value)
                {
                    dataGrid.SelectionChanged += DataGrid_SelectionChanged;
                }
            }
        }

        private static void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                SetSelectedItems(dataGrid, dataGrid.SelectedItems);
            }
        }

        private static void CleanUp(INotifyCollectionChanged? notifyCollectionChanged)
        {
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged -= DataGridAttach_CollectionChanged;
                CollectionToDataGridMap.Remove(notifyCollectionChanged);
            }
        }

        private static void OnAutoScrollToEndChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not DataGrid dataGrid)
            {
                return;
            }

            dataGrid.PropertyChanged -= DataGrid_PropertyChanged;
            if (e.NewValue.Value)
            {
                dataGrid.PropertyChanged += DataGrid_PropertyChanged;
            }

            if (dataGrid.ItemsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                if (e.OldValue.Value)
                {
                    CleanUp(notifyCollectionChanged);
                }

                if (e.NewValue.Value)
                {
                    CollectionToDataGridMap.AddOrUpdate(notifyCollectionChanged, new WeakReference<DataGrid>(dataGrid));
                    notifyCollectionChanged.CollectionChanged += DataGridAttach_CollectionChanged;
                }
            }
        }

        private static void DataGrid_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == DataGrid.ItemsSourceProperty && sender is DataGrid dataGrid)
            {
                CleanUp(e.OldValue as INotifyCollectionChanged);

                var args = new AvaloniaPropertyChangedEventArgs<bool>(dataGrid, AutoScrollToEndProperty, new Optional<bool>(true), new BindingValue<bool>(GetAutoScrollToEnd(dataGrid)), BindingPriority.LocalValue);
                OnAutoScrollToEndChanged(args);

                if (GetAutoScrollToEnd(dataGrid) && dataGrid.ItemsSource is IList list)
                {
                    ScrollToEnd(dataGrid, list);
                }
            }
        }

        private static void DataGridAttach_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null && CollectionToDataGridMap.TryGetValue(sender, out var dataGrid))
            {
                if (dataGrid.TryGetTarget(out var actualDataGrid))
                {
                    ScrollToEnd(actualDataGrid, e.NewItems);
                }
                else
                {
                    CleanUp(sender as INotifyCollectionChanged);
                }
            }
        }

        private static void ScrollToEnd(DataGrid dataGrid, IList? list)
        {
            if (GetIgnoreAutoScrollOnPointerOver(dataGrid) && dataGrid.IsPointerOver)
            {
                return;
            }
            if (list != null && list.Count > 0)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    dataGrid.ScrollIntoView(list[^1], null);
                }, DispatcherPriority.Loaded);
            }
        }

        private static void OnIsAllExpandedChanged(AvaloniaPropertyChangedEventArgs<bool?> e)
        {
            if (e.Sender is DataGrid dataGrid && dataGrid.ItemsSource is DataGridCollectionView cv && e.NewValue.Value is { } newValue)
            {
                var collectionViewGroups = cv.Groups.OfType<DataGridCollectionViewGroup>();
                foreach (var group in collectionViewGroups)
                {
                    //dataGrid.ScrollIntoView(group, null);
                    try
                    {
                        if (newValue)
                        {
                            dataGrid.ExpandRowGroup(group, expandAllSubgroups: true);
                        }
                        else
                        {
                            dataGrid.CollapseRowGroup(group, collapseAllSubgroups: true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                if (collectionViewGroups.Any())
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        //dataGrid.ScrollIntoView(collectionViewGroups.First(), null);
                        cv.Refresh();
                    }, DispatcherPriority.Loaded);
                }
            }
        }
    }
}