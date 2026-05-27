using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;

namespace Synthora.Attaches
{
    public class DataGridAttach
    {
        private static readonly ConcurrentDictionary<object, DataGrid> CollectionToDataGridMap = [];

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

        static DataGridAttach()
        {
            SelectedItemsAttachProperty.Changed.AddClassHandler<DataGrid, bool>((s, e) => SelectedItemsAttachChanged(e));
            ScrollIntoItemProperty.Changed.AddClassHandler<DataGrid, object?>((s, e) => OnScrollIntoItemChanged(e));
            AutoScrollToEndProperty.Changed.AddClassHandler<DataGrid, bool>((s, e) => OnAutoScrollToEndChanged(e));
        }

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

        private static void OnAutoScrollToEndChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not DataGrid dataGrid)
            {
                return;
            }

            dataGrid.PropertyChanged -= DataGrid_PropertyChanged;
            dataGrid.PropertyChanged += DataGrid_PropertyChanged;

            //DependencyPropertyDescriptor property = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(Selector));
            //property?.RemoveValueChanged(dataGrid, OnItemsSourceChanged);
            //property?.AddValueChanged(dataGrid, OnItemsSourceChanged);

            if (dataGrid.ItemsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                if (e.OldValue.Value)
                {
                    notifyCollectionChanged.CollectionChanged -= DataGridAttach_CollectionChanged;
                    CollectionToDataGridMap.TryRemove(notifyCollectionChanged, out _);
                }

                if (e.NewValue.Value)
                {
                    CollectionToDataGridMap[notifyCollectionChanged] = dataGrid;
                    notifyCollectionChanged.CollectionChanged += DataGridAttach_CollectionChanged;
                }
            }
        }

        private static void DataGrid_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == DataGrid.ItemsSourceProperty && sender is DataGrid dataGrid)
            {
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
                ScrollToEnd(dataGrid, e.NewItems);
            }
        }

        private static void ScrollToEnd(DataGrid dataGrid, IList? list)
        {
            if (GetIgnoreAutoScrollOnPointerOver(dataGrid) && dataGrid.IsPointerOver)
            {
                return;
            }
            if (list is { Count: > 0 })
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    dataGrid.ScrollIntoView(list[^1], null);
                }, DispatcherPriority.Loaded);
            }
        }
    }
}