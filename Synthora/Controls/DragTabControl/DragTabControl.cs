using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Synthora.Commands;
using Synthora.Events;
using Synthora.Extensions;

namespace Synthora.Controls
{
    public class DragTabControl : TabControl
    {
        private const double DefaultTabWidth = 140;

        public const double WindowsAndLinuxDefaultLeftThumbWidth = 0d;
        public const double MacOsDefaultLeftThumbWidth = 80d;

        public const double WindowsAndLinuxDefaultRightThumbWidth = 160d;
        public const double MacOsDefaultRightThumbWidth = 0d;

        private readonly TabsPanel _tabsPanel;

        private DragTabItem? _draggedItem;
        private bool _dragging;

        private ICommand _addItemCommand;
        private ICommand _closeItemCommand;

        public static readonly StyledProperty<double> AdjacentHeaderItemOffsetProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(AdjacentHeaderItemOffset), defaultValue: 0);


        public static readonly StyledProperty<double> TabItemWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(TabItemWidth), defaultValue: DefaultTabWidth);


        public static readonly StyledProperty<bool> ShowCloseButtonProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowCloseButton), defaultValue: true);


        public static readonly StyledProperty<bool> ShowAddButtonProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowAddButton), defaultValue: true);


        public static readonly StyledProperty<int> FixedHeaderCountProperty =
            AvaloniaProperty.Register<DragTabControl, int>(nameof(FixedHeaderCount), defaultValue: 0);


        public static readonly StyledProperty<Func<Task<object>>?> NewItemAsyncFactoryProperty =
            AvaloniaProperty.Register<DragTabControl, Func<Task<object>>?>(nameof(NewItemAsyncFactory));


        public static readonly StyledProperty<Func<object>?> NewItemFactoryProperty =
            AvaloniaProperty.Register<DragTabControl, Func<object>?>(nameof(NewItemFactory));


        public static readonly StyledProperty<EventHandler<TabClosedEventArgs>?> TabClosedProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<TabClosedEventArgs>?>(nameof(TabClosed));

        public static readonly StyledProperty<EventHandler<TabClosingEventArgs>?> TabClosingProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<TabClosingEventArgs>?>(nameof(TabClosing));


        public static readonly StyledProperty<EventHandler<CloseLastTabEventArgs>?> LastTabClosedActionProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<CloseLastTabEventArgs>?>(nameof(LastTabClosedAction));

        public static readonly StyledProperty<double> LeftThumbMinWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(LeftThumbMinWidth),
                defaultValue: OperatingSystem.IsMacOS() ? MacOsDefaultLeftThumbWidth : WindowsAndLinuxDefaultLeftThumbWidth);

        public static readonly StyledProperty<double> RightThumbMinWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(RightThumbMinWidth),
                defaultValue: OperatingSystem.IsMacOS() ? MacOsDefaultRightThumbWidth : WindowsAndLinuxDefaultRightThumbWidth);


        public static readonly DirectProperty<DragTabControl, ICommand> AddItemCommandProperty =
            AvaloniaProperty.RegisterDirect<DragTabControl, ICommand>(
                nameof(AddItemCommand),
                o => o.AddItemCommand,
                (o, v) => o.AddItemCommand = v);


        public static readonly DirectProperty<DragTabControl, ICommand> CloseItemCommandProperty =
            AvaloniaProperty.RegisterDirect<DragTabControl, ICommand>(
                nameof(CloseItemCommand),
                o => o.CloseItemCommand,
                (o, v) => o.CloseItemCommand = v);

        public static readonly StyledProperty<object?> LeftContentProperty =
            AvaloniaProperty.Register<DragTabControl, object?>(nameof(LeftContent));

        public static readonly StyledProperty<object?> RightContentProperty =
            AvaloniaProperty.Register<DragTabControl, object?>(nameof(RightContent));

        public static readonly StyledProperty<bool> ShowDragWindowThumbProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowDragWindowThumb));

        public DragTabControl()
        {
            AddHandler(DragTabItem.DragStarted, ItemDragStarted, handledEventsToo: true);
            AddHandler(DragTabItem.DragDelta, ItemDragDelta);
            AddHandler(DragTabItem.DragCompleted, ItemDragCompleted, handledEventsToo: true);

            _tabsPanel = new TabsPanel(this)
            {
                ItemWidth = TabItemWidth,
                ItemOffset = AdjacentHeaderItemOffset
            };

            _tabsPanel.DragCompleted += TabsPanelOnDragCompleted;

            ItemsPanel = new FuncTemplate<Panel?>(() => _tabsPanel);

            LastTabClosedAction = (_, _) => GetThisWindow()?.Close();

            _addItemCommand = new RelayCommand(AddItem);
            _closeItemCommand = new RelayCommand<object?>(CloseItem);
        }

        public double AdjacentHeaderItemOffset
        {
            get => GetValue(AdjacentHeaderItemOffsetProperty);
            set => SetValue(AdjacentHeaderItemOffsetProperty, value);
        }

        public double TabItemWidth
        {
            get => GetValue(TabItemWidthProperty);
            set => SetValue(TabItemWidthProperty, value);
        }

        public bool ShowCloseButton
        {
            get => GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        public bool ShowAddButton
        {
            get => GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, value);
        }

        public Func<Task<object>>? NewItemAsyncFactory
        {
            get => GetValue(NewItemAsyncFactoryProperty);
            set => SetValue(NewItemAsyncFactoryProperty, value);
        }

        public Func<object>? NewItemFactory
        {
            get => GetValue(NewItemFactoryProperty);
            set => SetValue(NewItemFactoryProperty, value);
        }

        public EventHandler<TabClosedEventArgs>? TabClosed
        {
            get => GetValue(TabClosedProperty);
            set => SetValue(TabClosedProperty, value);
        }

        public EventHandler<TabClosingEventArgs>? TabClosing
        {
            get => GetValue(TabClosingProperty);
            set => SetValue(TabClosingProperty, value);
        }

        public EventHandler<CloseLastTabEventArgs>? LastTabClosedAction
        {
            get => GetValue(LastTabClosedActionProperty);
            set => SetValue(LastTabClosedActionProperty, value);
        }

        /// <summary>
        /// Allows a specified number of leading tabs to be fixed (cannot be dragged, and the default close button is hidden).
        /// </summary>
        public int FixedHeaderCount
        {
            get => GetValue(FixedHeaderCountProperty);
            set => SetValue(FixedHeaderCountProperty, value);
        }

        public double LeftThumbMinWidth
        {
            get => GetValue(LeftThumbMinWidthProperty);
            set => SetValue(LeftThumbMinWidthProperty, value);
        }

        public double RightThumbMinWidth
        {
            get => GetValue(RightThumbMinWidthProperty);
            set => SetValue(RightThumbMinWidthProperty, value);
        }

        public ICommand AddItemCommand
        {
            get => _addItemCommand;
            private set => SetAndRaise(AddItemCommandProperty, ref _addItemCommand, value);
        }

        public ICommand CloseItemCommand
        {
            get => _closeItemCommand;
            private set => SetAndRaise(CloseItemCommandProperty, ref _closeItemCommand, value);
        }

        public object? LeftContent
        {
            get => GetValue(LeftContentProperty);
            set => SetValue(LeftContentProperty, value);
        }

        public object? RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }

        public bool ShowDragWindowThumb
        {
            get => GetValue(ShowDragWindowThumbProperty);
            set => SetValue(ShowDragWindowThumbProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var leftDragWindowThumb = e.NameScope.Get<Thumb>("PART_LeftDragWindowThumb");
            leftDragWindowThumb.AddHandler(PointerPressedEvent, OnThumbBeginDrag, handledEventsToo: true);
            //leftDragWindowThumb.DragDelta += WindowDragThumbOnDragDelta;
            leftDragWindowThumb.DoubleTapped += WindowDragThumbOnDoubleTapped;

            var rightDragWindowThumb = e.NameScope.Get<Thumb>("PART_RightDragWindowThumb");
            rightDragWindowThumb.AddHandler(PointerPressedEvent, OnThumbBeginDrag, handledEventsToo: true);
            // rightDragWindowThumb.DragDelta += WindowDragThumbOnDragDelta;
            rightDragWindowThumb.DoubleTapped += WindowDragThumbOnDoubleTapped;
        }

        protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new DragTabItem();

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == AdjacentHeaderItemOffsetProperty)
            {
                _tabsPanel.ItemOffset = AdjacentHeaderItemOffset;
            }
            else if (change.Property == TabItemWidthProperty)
            {
                _tabsPanel.ItemWidth = TabItemWidth;
            }
        }

        private IList GetItemsSource()
        {
            if (ItemsSource is IList list)
            {
                return list;
            }
            return ItemsView.Source;
        }

        private void RemoveItem(DragTabItem container)
        {
            object? item = ItemFromContainer(container);

            if (item == null || GetItemsSource() is not { } itemsList)
            {
                return;
            }

            int removedItemIndex = itemsList.IndexOf(item);

            if (removedItemIndex == -1)
            {
                return;
            }

            TabClosingEventArgs tabClosingEventArgs = new TabClosingEventArgs(item);
            TabClosing?.Invoke(this, tabClosingEventArgs);
            if (tabClosingEventArgs.Cancel)
            {
                return;
            }

            bool removedItemIsSelected = SelectedItem == item;

            itemsList.Remove(item);

            TabClosed?.Invoke(this, new TabClosedEventArgs(item));

            if (itemsList.Count == 0)
            {
                LastTabClosedAction?.Invoke(this, new CloseLastTabEventArgs(GetThisWindow()));
            }
            else if (removedItemIsSelected)
            {
                SetSelectedNewTab(itemsList, removedItemIndex);
            }
        }

        private void SetSelectedNewTab(IList items, int removedItemIndex) =>
            SelectedItem = removedItemIndex == items.Count ? items[^1] : items[removedItemIndex];

        private Window? GetThisWindow() => this.FindLogicalAncestorOfType<Window>();

        private IEnumerable<DragTabItem> DragTabItems()
        {
            foreach (var item in Items)
            {
                if (item != null && ContainerFromItem(item) is DragTabItem dragTabItem)
                {
                    yield return dragTabItem;
                }
            }
        }

        private void ItemDragStarted(object? sender, DragTabDragStartedEventArgs e)
        {
            _draggedItem = e.TabItem;

            e.Handled = true;

            _draggedItem.IsSelected = true;

            object? item = ItemFromContainer(_draggedItem);

            if (item != null)
            {
                if (item is TabItem tabItem)
                {
                    tabItem.IsSelected = true;
                }

                SelectedItem = item;
            }
        }

        private void ItemDragDelta(object? sender, DragTabDragDeltaEventArgs e)
        {
            if (_draggedItem is null)
            {
                throw new Exception($"{nameof(DragTabControl)}.{nameof(ItemDragDelta)} - _draggedItem is null");
            }

            if (_draggedItem.LogicalIndex < FixedHeaderCount)
            {
                e.Handled = true;
                return;
            }

            if (!_dragging)
            {
                _dragging = true;
                SetDraggingItem(_draggedItem);
            }

            _draggedItem.X += e.DragDeltaEventArgs.Vector.X;
            _draggedItem.Y += e.DragDeltaEventArgs.Vector.Y;

            Dispatcher.UIThread.Post(() => _tabsPanel.InvalidateMeasure(), DispatcherPriority.Loaded);

            e.Handled = true;
        }

        private void ItemDragCompleted(object? sender, DragTabDragCompletedEventArgs e)
        {
            foreach (var item in DragTabItems())
            {
                item.IsDragging = false;
                item.IsSiblingDragging = false;
            }

            Dispatcher.UIThread.Post(() => _tabsPanel.InvalidateMeasure(), DispatcherPriority.Loaded);

            _dragging = false;
        }

        private void SetDraggingItem(DragTabItem draggedItem)
        {
            foreach (var item in DragTabItems())
            {
                item.IsDragging = false;
                item.IsSiblingDragging = true;
            }

            draggedItem.IsDragging = true;
            draggedItem.IsSiblingDragging = false;
        }

        private void TabsPanelOnDragCompleted()
        {
            MoveTabModelsIfNeeded();

            _draggedItem = null;
        }

        private void MoveTabModelsIfNeeded()
        {
            if (_draggedItem == null)
            {
                return;
            }

            object? item = ItemFromContainer(_draggedItem);

            if (item != null)
            {
                DragTabItem container = _draggedItem;

                if (GetItemsSource() is { } list && container.LogicalIndex != list.IndexOf(item))
                {
                    list.Remove(item);
                    list.Insert(container.LogicalIndex, item);

                    SelectedItem = item;

                    int i = 0;

                    foreach (var dragTabItem in DragTabItems())
                    {
                        dragTabItem.LogicalIndex = i++;
                    }
                }
            }
        }

        private void OnThumbBeginDrag(object? sender, PointerPressedEventArgs e)
        {
            var toplevel = TopLevel.GetTopLevel(this);
            if (toplevel is not Window window)
            {
                return;
            }

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed ||
                e.GetCurrentPoint(this).Pointer.Type == PointerType.Touch)
            {
                window.BeginMoveDrag(e);
            }
        }

        private void WindowDragThumbOnDoubleTapped(object? sender, RoutedEventArgs e)
        {
            var window = this.FindLogicalAncestorOfType<Window>();

            window?.RestoreWindow();
        }

        [Obsolete]
        private void WindowDragThumbOnDragDelta(object? sender, VectorEventArgs e)
        {
            var window = this.FindLogicalAncestorOfType<Window>();

            window?.DragWindow(e.Vector.X, e.Vector.Y);
        }

        private void AddItem()
        {
            if (NewItemAsyncFactory is not null)
            {
                NewItemAsyncFactory.Invoke().ContinueWith(t => { AddItemCore(t.Result); },
                    scheduler: TaskScheduler.FromCurrentSynchronizationContext());

                return;
            }

            AddItemCore(NewItemFactory?.Invoke());
        }

        private void AddItemCore(object? newItem)
        {
            if (newItem == null)
            {
                return;
            }
            //ArgumentNullException.ThrowIfNull(newItem);

            if (GetItemsSource() is { } itemsList)
            {
                itemsList.Add(newItem);
            }

            SelectedItem = newItem;
        }

        private void CloseItem(object? tabItemSource)
        {
            ArgumentNullException.ThrowIfNull(tabItemSource);

            if (tabItemSource is not DragTabItem tabItem)
            {
                return;
            }

            RemoveItem(tabItem);
        }
    }
}