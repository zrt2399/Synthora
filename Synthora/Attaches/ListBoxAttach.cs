using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Synthora.Attaches
{
    public class ListBoxAttach
    {
        private static readonly ConcurrentDictionary<object, ListBox> CollectionToListBoxMap = [];

        public static readonly AttachedProperty<bool> AutoScrollToEndProperty =
            AvaloniaProperty.RegisterAttached<ListBoxAttach, ListBox, bool>("AutoScrollToEnd");

        public static readonly AttachedProperty<bool> IgnoreAutoScrollOnPointerOverProperty =
            AvaloniaProperty.RegisterAttached<ListBoxAttach, ListBox, bool>("IgnoreAutoScrollOnPointerOver");

        static ListBoxAttach()
        {
            AutoScrollToEndProperty.Changed.AddClassHandler<ListBox, bool>((s, e) => OnAutoScrollToEndChanged(e));
        }

        public static bool GetAutoScrollToEnd(ListBox obj) => obj.GetValue(AutoScrollToEndProperty);
        public static void SetAutoScrollToEnd(ListBox obj, bool value) => obj.SetValue(AutoScrollToEndProperty, value);

        public static bool GetIgnoreAutoScrollOnPointerOver(ListBox obj) => obj.GetValue(IgnoreAutoScrollOnPointerOverProperty);
        public static void SetIgnoreAutoScrollOnPointerOver(ListBox obj, bool value) => obj.SetValue(IgnoreAutoScrollOnPointerOverProperty, value);

        private static void OnAutoScrollToEndChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not ListBox listBox)
            {
                return;
            }

            listBox.PropertyChanged -= ListBox_PropertyChanged;
            listBox.PropertyChanged += ListBox_PropertyChanged;

            if (listBox.ItemsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                if (e.OldValue.Value)
                {
                    notifyCollectionChanged.CollectionChanged -= ListBoxAttach_CollectionChanged;
                    CollectionToListBoxMap.TryRemove(notifyCollectionChanged, out _);
                }

                if (e.NewValue.Value)
                {
                    CollectionToListBoxMap[notifyCollectionChanged] = listBox;
                    notifyCollectionChanged.CollectionChanged += ListBoxAttach_CollectionChanged;
                }
            }
        }

        private static void ListBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == ItemsControl.ItemsSourceProperty && sender is ListBox listBox)
            {
                var args = new AvaloniaPropertyChangedEventArgs<bool>(listBox, AutoScrollToEndProperty, new Optional<bool>(true), new BindingValue<bool>(GetAutoScrollToEnd(listBox)), BindingPriority.LocalValue);

                OnAutoScrollToEndChanged(args);

                if (GetAutoScrollToEnd(listBox))
                {
                    ScrollToEnd(listBox);
                }
            }
        }

        private static void ListBoxAttach_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null && CollectionToListBoxMap.TryGetValue(sender, out var listBox) && e.Action == NotifyCollectionChangedAction.Add)
            {
                ScrollToEnd(listBox);
            }
        }

        private static void ScrollToEnd(ListBox listBox)
        {
            if (GetIgnoreAutoScrollOnPointerOver(listBox) && listBox.IsPointerOver)
            {
                return;
            }
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var scrollViewer = listBox.GetVisualDescendants().OfType<ScrollViewer>().FirstOrDefault();
                scrollViewer?.ScrollToEnd();
            }, DispatcherPriority.Loaded);
        }
    }
}