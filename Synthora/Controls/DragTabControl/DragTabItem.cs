using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Synthora.Events;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a draggable tab item used by <see cref="DragTabControl"/>.
    /// </summary>
    public class DragTabItem : TabItem
    {
        private LeftPressedThumb? _thumb;

        private int _prevZindex;
        private int _logicalIndex;
        private bool _isDragging;
        private bool _isSiblingDragging;

        /// <summary>
        /// Defines the <see cref="X"/> property.
        /// </summary>
        public static readonly StyledProperty<double> XProperty =
            AvaloniaProperty.Register<DragTabItem, double>(nameof(X));

        /// <summary>
        /// Defines the <see cref="Y"/> property.
        /// </summary>
        public static readonly StyledProperty<double> YProperty =
            AvaloniaProperty.Register<DragTabItem, double>(nameof(Y));

        /// <summary>
        /// Defines the <see cref="IsDragging"/> property.
        /// </summary>
        public static readonly DirectProperty<DragTabItem, bool> IsDraggingProperty =
            AvaloniaProperty.RegisterDirect<DragTabItem, bool>(nameof(IsDragging), o => o.IsDragging);

        /// <summary>
        /// Defines the <see cref="LogicalIndex"/> property.
        /// </summary>
        public static readonly DirectProperty<DragTabItem, int> LogicalIndexProperty =
            AvaloniaProperty.RegisterDirect<DragTabItem, int>(nameof(LogicalIndex), o => o.LogicalIndex);

        /// <summary>
        /// Defines the <see cref="IsSiblingDragging"/> property.
        /// </summary>
        public static readonly DirectProperty<DragTabItem, bool> IsSiblingDraggingProperty =
            AvaloniaProperty.RegisterDirect<DragTabItem, bool>(nameof(IsSiblingDragging), o => o.IsSiblingDragging);

        /// <summary>
        /// Gets or sets the arranged x-coordinate of the tab item.
        /// </summary>
        public double X
        {
            get => GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        /// <summary>
        /// Gets or sets the arranged y-coordinate of the tab item.
        /// </summary>
        public double Y
        {
            get => GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        /// <summary>
        /// Gets the logical index of the tab item within the header panel.
        /// </summary>
        public int LogicalIndex
        {
            get => _logicalIndex;
            internal set => SetAndRaise(LogicalIndexProperty, ref _logicalIndex, value);
        }

        /// <summary>
        /// Gets a value indicating whether this tab item is currently being dragged.
        /// </summary>
        public bool IsDragging
        {
            get => _isDragging;
            internal set => SetAndRaise(IsDraggingProperty, ref _isDragging, value);
        }

        /// <summary>
        /// Gets a value indicating whether another tab item in the same control is being dragged.
        /// </summary>
        public bool IsSiblingDragging
        {
            get => _isSiblingDragging;
            internal set => SetAndRaise(IsSiblingDraggingProperty, ref _isSiblingDragging, value);
        }

        /// <summary>
        /// Identifies the routed event raised when tab dragging starts.
        /// </summary>
        public static readonly RoutedEvent<DragTabDragStartedEventArgs> DragStarted =
            RoutedEvent.Register<DragTabItem, DragTabDragStartedEventArgs>("DragStarted", RoutingStrategies.Bubble);

        /// <summary>
        /// Identifies the routed event raised when tab dragging moves.
        /// </summary>
        public static readonly RoutedEvent<DragTabDragDeltaEventArgs> DragDelta =
            RoutedEvent.Register<DragTabItem, DragTabDragDeltaEventArgs>("DragDelta", RoutingStrategies.Bubble);

        /// <summary>
        /// Identifies the routed event raised when tab dragging completes.
        /// </summary>
        public static readonly RoutedEvent<DragTabDragCompletedEventArgs> DragCompleted =
            RoutedEvent.Register<DragTabItem, DragTabDragCompletedEventArgs>("DragCompleted", RoutingStrategies.Bubble);

        /// <summary>
        /// Identifies the preview routed event raised before a drag delta is applied.
        /// </summary>
        public static readonly RoutedEvent<DragTabDragDeltaEventArgs> PreviewDragDelta =
            RoutedEvent.Register<DragTabItem, DragTabDragDeltaEventArgs>("PreviewDragDelta", RoutingStrategies.Tunnel);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            UnregisterEvents();

            _thumb = e.NameScope.Find<LeftPressedThumb>("PART_Thumb");

            if (_thumb != null)
            {
                _thumb.DragStarted += ThumbOnDragStarted;
                _thumb.DragDelta += ThumbOnDragDelta;
                _thumb.DragCompleted += ThumbOnDragCompleted;
            }
        }

        protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);

            if (IsSelected || IsDragging)
            {
                return;
            }

            _prevZindex = ZIndex;

            ZIndex = ZIndexes.PointerOver;
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);

            if (IsSelected || IsDragging)
            {
                return;
            }

            ZIndex = _prevZindex;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == IsSelectedProperty)
            {
                ZIndex = change.NewValue is true ? ZIndexes.Selected : ZIndexes.NonSelected;
            }
        }

        public override string ToString()
        {
            return $"{nameof(DragTabItem)}.{nameof(Header)}:{Header}";
        }

        private void UnregisterEvents()
        {
            if (_thumb != null)
            {
                _thumb.DragStarted -= ThumbOnDragStarted;
                _thumb.DragDelta -= ThumbOnDragDelta;
                _thumb.DragCompleted -= ThumbOnDragCompleted;
            }
        }

        private void ThumbOnDragStarted(object? sender, VectorEventArgs args)
        {
            RaiseEvent(new DragTabDragStartedEventArgs(DragStarted, this, args));
        }

        private void ThumbOnDragDelta(object? sender, VectorEventArgs e)
        {
            var previewEventArgs = new DragTabDragDeltaEventArgs(PreviewDragDelta, this, e);
            RaiseEvent(previewEventArgs);
            // if (previewEventArgs.Cancel)
            //     _thumb.CancelDrag();
            if (!previewEventArgs.Handled)
            {
                var eventArgs = new DragTabDragDeltaEventArgs(DragDelta, this, e);
                RaiseEvent(eventArgs);
                //if (eventArgs.Cancel)
                //    thumb.CancelDrag();
            }
        }

        private void ThumbOnDragCompleted(object? sender, VectorEventArgs e)
        {
            var args = new DragTabDragCompletedEventArgs(DragCompleted, this, e);
            RaiseEvent(args);
        }
    }

    /// <summary>
    /// Provides z-index constants used by draggable tab headers.
    /// </summary>
    public static class ZIndexes
    {
        /// <summary>
        /// Gets the z-index assigned to the selected tab item.
        /// </summary>
        public const int Selected = int.MaxValue;

        /// <summary>
        /// Gets the z-index assigned to a pointer-over tab item.
        /// </summary>
        public const int PointerOver = Selected - 1;

        /// <summary>
        /// Gets the z-index assigned to a non-selected tab item.
        /// </summary>
        public const int NonSelected = Selected - 10;
    }
}