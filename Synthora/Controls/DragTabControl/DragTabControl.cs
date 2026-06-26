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
using Synthora.Commands;
using Synthora.Events;
using Synthora.Extensions;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a tab control whose tab headers can be reordered by dragging.
    /// </summary>
    public class DragTabControl : TabControl
    {
        private const double DefaultTabWidth = 140;

        /// <summary>
        /// Gets the default left drag-thumb width on Windows and Linux.
        /// </summary>
        public const double WindowsAndLinuxDefaultLeftThumbWidth = 0d;

        /// <summary>
        /// Gets the default left drag-thumb width on macOS.
        /// </summary>
        public const double MacOsDefaultLeftThumbWidth = 80d;

        /// <summary>
        /// Gets the default right drag-thumb width on Windows and Linux.
        /// </summary>
        public const double WindowsAndLinuxDefaultRightThumbWidth = 160d;

        /// <summary>
        /// Gets the default right drag-thumb width on macOS.
        /// </summary>
        public const double MacOsDefaultRightThumbWidth = 0d;

        private Button? _addItemButton;
        private Thumb? _leftDragWindowThumb;
        private Thumb? _rightDragWindowThumb;
        private readonly TabsPanel _tabsPanel;
        private ICommand _addItemCommand;
        private ICommand _closeItemCommand;

        private DragTabItem? _draggedItem;
        private bool _dragging;

        /// <summary>
        /// Defines the <see cref="AdjacentHeaderItemOffset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> AdjacentHeaderItemOffsetProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(AdjacentHeaderItemOffset));

        /// <summary>
        /// Defines the <see cref="TabItemWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TabItemWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(TabItemWidth), defaultValue: DefaultTabWidth);

        /// <summary>
        /// Defines the <see cref="ShowCloseButton"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowCloseButtonProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowCloseButton), defaultValue: true);

        /// <summary>
        /// Defines the <see cref="ShowAddButton"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowAddButtonProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowAddButton), defaultValue: true);

        /// <summary>
        /// Defines the <see cref="FixedHeaderCount"/> property.
        /// </summary>
        public static readonly StyledProperty<int> FixedHeaderCountProperty =
            AvaloniaProperty.Register<DragTabControl, int>(nameof(FixedHeaderCount));

        /// <summary>
        /// Defines the <see cref="NewItemAsyncFactory"/> property.
        /// </summary>
        public static readonly StyledProperty<Func<Task<object>>?> NewItemAsyncFactoryProperty =
            AvaloniaProperty.Register<DragTabControl, Func<Task<object>>?>(nameof(NewItemAsyncFactory));

        /// <summary>
        /// Defines the <see cref="NewItemFactory"/> property.
        /// </summary>
        public static readonly StyledProperty<Func<object>?> NewItemFactoryProperty =
            AvaloniaProperty.Register<DragTabControl, Func<object>?>(nameof(NewItemFactory));

        /// <summary>
        /// Defines the <see cref="AddButtonFlyout"/> property.
        /// </summary>
        public static readonly StyledProperty<FlyoutBase?> AddButtonFlyoutProperty =
            AvaloniaProperty.Register<DragTabControl, FlyoutBase?>(nameof(AddButtonFlyout));

        /// <summary>
        /// Defines the <see cref="TabClosed"/> property.
        /// </summary>
        public static readonly StyledProperty<EventHandler<TabClosedEventArgs>?> TabClosedProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<TabClosedEventArgs>?>(nameof(TabClosed));

        /// <summary>
        /// Defines the <see cref="TabClosing"/> property.
        /// </summary>
        public static readonly StyledProperty<EventHandler<TabClosingEventArgs>?> TabClosingProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<TabClosingEventArgs>?>(nameof(TabClosing));

        /// <summary>
        /// Defines the <see cref="LastTabClosedAction"/> property.
        /// </summary>
        public static readonly StyledProperty<EventHandler<CloseLastTabEventArgs>?> LastTabClosedActionProperty =
            AvaloniaProperty.Register<DragTabControl, EventHandler<CloseLastTabEventArgs>?>(nameof(LastTabClosedAction));

        /// <summary>
        /// Defines the <see cref="LeftThumbMinWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> LeftThumbMinWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(LeftThumbMinWidth),
                defaultValue: OperatingSystem.IsMacOS() ? MacOsDefaultLeftThumbWidth : WindowsAndLinuxDefaultLeftThumbWidth);

        /// <summary>
        /// Defines the <see cref="RightThumbMinWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> RightThumbMinWidthProperty =
            AvaloniaProperty.Register<DragTabControl, double>(nameof(RightThumbMinWidth),
                defaultValue: OperatingSystem.IsMacOS() ? MacOsDefaultRightThumbWidth : WindowsAndLinuxDefaultRightThumbWidth);

        /// <summary>
        /// Defines the <see cref="AddItemCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<DragTabControl, ICommand> AddItemCommandProperty =
            AvaloniaProperty.RegisterDirect<DragTabControl, ICommand>(nameof(AddItemCommand), o => o.AddItemCommand);

        /// <summary>
        /// Defines the <see cref="CloseItemCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<DragTabControl, ICommand> CloseItemCommandProperty =
            AvaloniaProperty.RegisterDirect<DragTabControl, ICommand>(nameof(CloseItemCommand), o => o.CloseItemCommand);

        /// <summary>
        /// Defines the <see cref="LeftContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> LeftContentProperty =
            AvaloniaProperty.Register<DragTabControl, object?>(nameof(LeftContent));

        /// <summary>
        /// Defines the <see cref="RightContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> RightContentProperty =
            AvaloniaProperty.Register<DragTabControl, object?>(nameof(RightContent));

        /// <summary>
        /// Defines the <see cref="ShowDragWindowThumb"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowDragWindowThumbProperty =
            AvaloniaProperty.Register<DragTabControl, bool>(nameof(ShowDragWindowThumb));

        /// <summary>
        /// Initializes a new instance of the <see cref="DragTabControl"/> class.
        /// </summary>
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

            // LastTabClosedAction = (_, _) => GetThisWindow()?.Close();

            _addItemCommand = new RelayCommand(AddItem);
            _closeItemCommand = new RelayCommand<object?>(CloseItem);
        }

        /// <summary>
        /// Gets or sets the spacing offset between adjacent tab headers.
        /// </summary>
        public double AdjacentHeaderItemOffset
        {
            get => GetValue(AdjacentHeaderItemOffsetProperty);
            set => SetValue(AdjacentHeaderItemOffsetProperty, value);
        }

        /// <summary>
        /// Gets or sets the preferred width of each tab item.
        /// </summary>
        public double TabItemWidth
        {
            get => GetValue(TabItemWidthProperty);
            set => SetValue(TabItemWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets whether close buttons are shown on tab items.
        /// </summary>
        public bool ShowCloseButton
        {
            get => GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the add-tab button is shown.
        /// </summary>
        public bool ShowAddButton
        {
            get => GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets the asynchronous factory used to create new tab items.
        /// </summary>
        public Func<Task<object>>? NewItemAsyncFactory
        {
            get => GetValue(NewItemAsyncFactoryProperty);
            set => SetValue(NewItemAsyncFactoryProperty, value);
        }

        /// <summary>
        /// Gets or sets the synchronous factory used to create new tab items.
        /// </summary>
        public Func<object>? NewItemFactory
        {
            get => GetValue(NewItemFactoryProperty);
            set => SetValue(NewItemFactoryProperty, value);
        }

        /// <summary>
        /// Gets or sets the flyout attached to the add-tab button.
        /// </summary>
        public FlyoutBase? AddButtonFlyout
        {
            get => GetValue(AddButtonFlyoutProperty);
            set => SetValue(AddButtonFlyoutProperty, value);
        }

        /// <summary>
        /// Gets or sets the callback invoked after a tab is closed.
        /// </summary>
        public EventHandler<TabClosedEventArgs>? TabClosed
        {
            get => GetValue(TabClosedProperty);
            set => SetValue(TabClosedProperty, value);
        }

        /// <summary>
        /// Gets or sets the callback invoked before a tab is closed.
        /// </summary>
        public EventHandler<TabClosingEventArgs>? TabClosing
        {
            get => GetValue(TabClosingProperty);
            set => SetValue(TabClosingProperty, value);
        }

        /// <summary>
        /// Gets or sets the callback invoked when the last tab is closed.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the minimum width of the left window drag thumb area.
        /// </summary>
        public double LeftThumbMinWidth
        {
            get => GetValue(LeftThumbMinWidthProperty);
            set => SetValue(LeftThumbMinWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum width of the right window drag thumb area.
        /// </summary>
        public double RightThumbMinWidth
        {
            get => GetValue(RightThumbMinWidthProperty);
            set => SetValue(RightThumbMinWidthProperty, value);
        }

        /// <summary>
        /// Gets the command that adds a new tab item.
        /// </summary>
        public ICommand AddItemCommand
        {
            get => _addItemCommand;
            private set => SetAndRaise(AddItemCommandProperty, ref _addItemCommand, value);
        }

        /// <summary>
        /// Gets the command that closes a tab item.
        /// </summary>
        public ICommand CloseItemCommand
        {
            get => _closeItemCommand;
            private set => SetAndRaise(CloseItemCommandProperty, ref _closeItemCommand, value);
        }

        /// <summary>
        /// Gets or sets the content displayed to the left of the tab headers.
        /// </summary>
        public object? LeftContent
        {
            get => GetValue(LeftContentProperty);
            set => SetValue(LeftContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed to the right of the tab headers.
        /// </summary>
        public object? RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }

        /// <summary>
        /// Gets or sets whether window drag thumb areas are shown.
        /// </summary>
        public bool ShowDragWindowThumb
        {
            get => GetValue(ShowDragWindowThumbProperty);
            set => SetValue(ShowDragWindowThumbProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            UnregisterEvents();

            _leftDragWindowThumb = e.NameScope.Find<Thumb>("PART_LeftDragWindowThumb");
            if (_leftDragWindowThumb != null)
            {
                _leftDragWindowThumb.AddHandler(PointerPressedEvent, OnThumbBeginDrag, handledEventsToo: true);
                //_leftDragWindowThumb.DragDelta += WindowDragThumbOnDragDelta;
                _leftDragWindowThumb.DoubleTapped += WindowDragThumbOnDoubleTapped;
            }

            _rightDragWindowThumb = e.NameScope.Find<Thumb>("PART_RightDragWindowThumb");
            if (_rightDragWindowThumb != null)
            {
                _rightDragWindowThumb.AddHandler(PointerPressedEvent, OnThumbBeginDrag, handledEventsToo: true);
                // _leftDragWindowThumb.DragDelta += WindowDragThumbOnDragDelta;
                _rightDragWindowThumb.DoubleTapped += WindowDragThumbOnDoubleTapped;
            }

            _addItemButton = e.NameScope.Find<Button>("PART_AddItemButton");
            if (_addItemButton != null)
            {
                _addItemButton.Click += AddItemButtonClick;
            }
        }

        protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        {
            return new DragTabItem();
        }

        protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
        {
            return NeedsContainer<DragTabItem>(item, out recycleKey);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == AdjacentHeaderItemOffsetProperty)
            {
                _tabsPanel.ItemOffset = AdjacentHeaderItemOffset;
                _tabsPanel.InvalidateMeasure();
            }
            else if (change.Property == TabItemWidthProperty)
            {
                _tabsPanel.ItemWidth = TabItemWidth;
                _tabsPanel.InvalidateMeasure();
            }
        }

        private void UnregisterEvents()
        {
            if (_leftDragWindowThumb != null)
            {
                _leftDragWindowThumb.RemoveHandler(PointerPressedEvent, OnThumbBeginDrag);
                _leftDragWindowThumb.DoubleTapped -= WindowDragThumbOnDoubleTapped;
            }
            if (_rightDragWindowThumb != null)
            {
                _rightDragWindowThumb.RemoveHandler(PointerPressedEvent, OnThumbBeginDrag);
                _rightDragWindowThumb.DoubleTapped -= WindowDragThumbOnDoubleTapped;
            }
            if (_addItemButton != null)
            {
                _addItemButton.Click -= AddItemButtonClick;
            }
        }

        private void AddItemButtonClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (AddButtonFlyout != null)
                {
                    AddButtonFlyout.ShowAt(button);
                }
                else
                {
                    AddItemCommand.Execute(null);
                }
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

            var tabClosingEventArgs = new TabClosingEventArgs(item);
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
            if (_dragging && _draggedItem != e.TabItem)
            {
                e.Handled = true;
                return;
            }

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
            if (_draggedItem is null || _draggedItem != e.TabItem)
            {
                e.Handled = true;
                return;
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

            _tabsPanel.InvalidateMeasure();

            e.Handled = true;
        }

        private void ItemDragCompleted(object? sender, DragTabDragCompletedEventArgs e)
        {
            if (_draggedItem is null || _draggedItem != e.TabItem)
            {
                e.Handled = true;
                return;
            }

            foreach (var item in DragTabItems())
            {
                item.IsDragging = false;
                item.IsSiblingDragging = false;
            }

            _tabsPanel.InvalidateMeasure();

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
            //ArgumentNullException.ThrowIfNull(tabItemSource);

            if (tabItemSource is not DragTabItem tabItem)
            {
                return;
            }

            RemoveItem(tabItem);
        }
    }
}