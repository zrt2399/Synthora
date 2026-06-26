using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Synthora.Controls
{
    /// <summary>
    /// Defines the side on which a tree menu item header is displayed.
    /// </summary>
    public enum HeaderPosition
    {
        /// <summary>
        /// Displays the header on the left side.
        /// </summary>
        Left,

        /// <summary>
        /// Displays the header on the right side.
        /// </summary>
        Right
    }

    /// <summary>
    /// Represents an item in a <see cref="TreeMenu"/>.
    /// </summary>
    [PseudoClasses(pcSelectedDescendant, pcExpanded, pcCurrentPressed)]
    public class TreeMenuItem : TreeViewItem
    {
        private const string pcSelectedDescendant = ":selected-descendant";
        private const string pcExpanded = ":expanded";
        private const string pcCurrentPressed = ":current-pressed";
        private readonly TimeSpan _expandCollapseDuration = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Defines the <see cref="HeaderPosition"/> property.
        /// </summary>
        public static readonly StyledProperty<HeaderPosition> HeaderPositionProperty =
            AvaloniaProperty.Register<TreeMenuItem, HeaderPosition>(nameof(HeaderPosition));

        /// <summary>
        /// Defines the <see cref="HasSelectedDescendant"/> property.
        /// </summary>
        public static readonly DirectProperty<TreeMenuItem, bool> HasSelectedDescendantProperty =
            AvaloniaProperty.RegisterDirect<TreeMenuItem, bool>(nameof(HasSelectedDescendant), o => o.HasSelectedDescendant);

        /// <summary>
        /// Defines the <see cref="Content"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> ContentProperty =
            ContentControl.ContentProperty.AddOwner<TreeMenuItem>();

        /// <summary>
        /// Defines the <see cref="ContentTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty =
            ContentControl.ContentTemplateProperty.AddOwner<TreeMenuItem>();

        private Control? _presenterRoot;
        private Control? _itemsContainer;
        private ItemsPresenter? _itemsPresenter;
        private CancellationTokenSource? _itemsAnimationCancellationTokenSource;
        private bool _hasSelectedDescendant;

        private bool HasChildItems => ItemsView.Count > 0;

        /// <summary>
        /// Gets or sets the side on which the header is displayed.
        /// </summary>
        public HeaderPosition HeaderPosition
        {
            get => GetValue(HeaderPositionProperty);
            set => SetValue(HeaderPositionProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether a descendant item is selected.
        /// </summary>
        public bool HasSelectedDescendant
        {
            get => _hasSelectedDescendant;
            internal set => SetAndRaise(HasSelectedDescendantProperty, ref _hasSelectedDescendant, value);
        }

        /// <summary>
        /// Gets or sets the content displayed when the item is selected.
        /// </summary>
        public object? Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display <see cref="Content"/>.
        /// </summary>
        public IDataTemplate? ContentTemplate
        {
            get => GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (ShouldSuppressSelection(e))
            {
                e.Handled = true;
                return;
            }

            base.OnPointerPressed(e);
        }

        protected override void OnHeaderDoubleTapped(TappedEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            RefreshSelfAndAncestors();
            Dispatcher.UIThread.Post(RefreshSelfAndAncestors, DispatcherPriority.Loaded);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            UnregisterEvents();
            CancelItemsAnimation();

            _presenterRoot = e.NameScope.Find<Control>("PART_PresenterRoot");
            _itemsContainer = e.NameScope.Find<Control>("PART_ItemsContainer");
            _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");

            if (_presenterRoot != null)
            {
                _presenterRoot.PointerPressed += PresenterRootPointerPressed;
                _presenterRoot.PointerReleased += PresenterRootPointerReleased;
                _presenterRoot.PointerCaptureLost += PresenterRootPointerCaptureLost;
                _presenterRoot.PropertyChanged += PresenterPropertyChanged;
                _presenterRoot.LostFocus += PresenterRootLostFocus;
            }

            ApplyItemsContainerState(IsExpanded);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            var parent = GetParentTreeMenuItem();

            CancelItemsAnimation();

            base.OnDetachedFromVisualTree(e);

            HasSelectedDescendant = false;

            for (var current = parent; current != null; current = current.GetParentTreeMenuItem())
            {
                current.RefreshHasSelectedDescendant();
            }
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsExpandedProperty)
            {
                AnimateItemsContainer(IsExpanded);
                PseudoClasses.Set(pcExpanded, IsExpanded);
            }
            else if (e.Property == IsSelectedProperty)
            {
                if (IsSelected && HasChildItems)
                {
                    SetCurrentValue(IsSelectedProperty, false);
                }
                else
                {
                    RefreshSelfAndAncestors();
                }
            }
            else if (e.Property == HasSelectedDescendantProperty)
            {
                PseudoClasses.Set(pcSelectedDescendant, HasSelectedDescendant);
            }
        }

        private void UnregisterEvents()
        {
            if (_presenterRoot != null)
            {
                _presenterRoot.PointerPressed -= PresenterRootPointerPressed;
                _presenterRoot.PointerReleased -= PresenterRootPointerReleased;
                _presenterRoot.PointerCaptureLost -= PresenterRootPointerCaptureLost;
                _presenterRoot.PropertyChanged -= PresenterPropertyChanged;
                _presenterRoot.LostFocus -= PresenterRootLostFocus;
            }
        }

        private void PresenterRootPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (_presenterRoot is null)
            {
                return;
            }

            PseudoClasses.Set(pcCurrentPressed, true);
        }

        private void PresenterRootPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            PseudoClasses.Set(pcCurrentPressed, false);

            if (HasChildItems)
            {
                SetCurrentValue(IsExpandedProperty, !IsExpanded);
            }
        }

        private void PresenterRootPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
        {
            PseudoClasses.Set(pcCurrentPressed, false);
        }

        private void PresenterPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == IsEffectivelyEnabledProperty && !e.GetNewValue<bool>())
            {
                PseudoClasses.Set(pcCurrentPressed, false);
            }
        }

        private void PresenterRootLostFocus(object? sender, FocusChangedEventArgs e)
        {
            PseudoClasses.Set(pcCurrentPressed, false);
        }

        private void RefreshSelfAndAncestors()
        {
            for (var current = this; current != null; current = current.GetParentTreeMenuItem())
            {
                current.RefreshHasSelectedDescendant();
            }
        }

        private void RefreshHasSelectedDescendant()
        {
            HasSelectedDescendant = HasSelectedDescendantCore();
        }

        private bool HasSelectedDescendantCore()
        {
            if (!HasChildItems)
            {
                return false;
            }

            foreach (var item in ItemsView)
            {
                if (item is null)
                {
                    continue;
                }

                if (ContainerFromItem(item) is not TreeMenuItem child)
                {
                    continue;
                }

                if (child.IsSelected || child.HasSelectedDescendantCore())
                {
                    return true;
                }
            }

            return false;
        }

        private TreeMenuItem? GetParentTreeMenuItem()
        {
            for (Visual? current = this; current != null; current = current.GetVisualParent())
            {
                if (current != this && current is TreeMenuItem parentItem)
                {
                    return parentItem;
                }
            }

            return null;
        }

        private async void AnimateItemsContainer(bool isExpanded)
        {
            if (_itemsContainer is null || _itemsPresenter is null)
            {
                return;
            }

            var cancellationToken = ResetItemsAnimationCancellationToken();

            try
            {
                var animationState = await GetItemsContainerAnimationStateAsync(isExpanded, cancellationToken);
                if (!animationState.HasAnimation)
                {
                    ApplyItemsContainerState(isExpanded);
                    return;
                }

                PrepareItemsContainerForAnimation(animationState.FromHeight, animationState.FromOpacity);
                await RunItemsContainerAnimationAsync(animationState, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                ApplyItemsContainerState(isExpanded);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void ApplyItemsContainerState(bool isExpanded)
        {
            if (_itemsContainer is null)
            {
                return;
            }

            _itemsContainer.IsVisible = isExpanded;
            _itemsContainer.Opacity = isExpanded ? 1 : 0;
            _itemsContainer.MaxHeight = isExpanded ? double.PositiveInfinity : 0;
        }

        private async Task<ItemsContainerAnimationState> GetItemsContainerAnimationStateAsync(bool isExpanded, CancellationToken cancellationToken)
        {
            if (_itemsContainer is null)
            {
                return default;
            }

            var currentOpacity = Math.Clamp(_itemsContainer.Opacity, 0, 1);

            if (isExpanded)
            {
                await Dispatcher.UIThread.InvokeAsync(static () => { }, DispatcherPriority.Render, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                var expandedHeight = GetExpandedHeight();
                if (expandedHeight <= 0)
                {
                    return default;
                }

                var currentHeight = Math.Clamp(_itemsContainer.Bounds.Height, 0, expandedHeight);
                return new ItemsContainerAnimationState(currentHeight, expandedHeight, currentOpacity, 1);
            }

            var collapsedHeight = Math.Max(_itemsContainer.Bounds.Height, 0);
            return collapsedHeight <= 0
                ? default
                : new ItemsContainerAnimationState(collapsedHeight, 0, currentOpacity, 0);
        }

        private double GetExpandedHeight()
        {
            if (_itemsPresenter is null)
            {
                return 0;
            }

            var availableWidth = _itemsContainer?.Bounds.Width ?? 0;
            if (availableWidth <= 0)
            {
                availableWidth = Bounds.Width;
            }

            _itemsPresenter.Measure(new Size(availableWidth > 0 ? availableWidth : double.PositiveInfinity, double.PositiveInfinity));

            return _itemsPresenter.DesiredSize.Height;
        }

        private void PrepareItemsContainerForAnimation(double fromHeight, double fromOpacity)
        {
            if (_itemsContainer is null)
            {
                return;
            }

            _itemsContainer.IsVisible = true;
            _itemsContainer.MaxHeight = fromHeight;
            _itemsContainer.Opacity = fromOpacity;
        }

        private Task RunItemsContainerAnimationAsync(ItemsContainerAnimationState animationState, CancellationToken cancellationToken)
        {
            if (_itemsContainer is null)
            {
                return Task.CompletedTask;
            }

            var animation = new Animation
            {
                Easing = new CubicEaseOut(),
                Duration = _expandCollapseDuration,
                FillMode = FillMode.Forward,
                Children =
                {
                    new KeyFrame
                    {
                        KeyTime = _expandCollapseDuration,
                        Setters =
                        {
                            new Setter(MaxHeightProperty, animationState.ToHeight),
                            new Setter(OpacityProperty, animationState.ToOpacity)
                        }
                    }
                }
            };

            return animation.RunAsync(_itemsContainer, cancellationToken);
        }

        private CancellationToken ResetItemsAnimationCancellationToken()
        {
            CancelItemsAnimation();
            _itemsAnimationCancellationTokenSource = new CancellationTokenSource();
            return _itemsAnimationCancellationTokenSource.Token;
        }

        private void CancelItemsAnimation()
        {
            _itemsAnimationCancellationTokenSource?.Cancel();
            _itemsAnimationCancellationTokenSource?.Dispose();
            _itemsAnimationCancellationTokenSource = null;
        }

        private bool ShouldSuppressSelection(PointerPressedEventArgs e)
        {
            return HasChildItems;
        }

        private readonly record struct ItemsContainerAnimationState(
            double FromHeight,
            double ToHeight,
            double FromOpacity,
            double ToOpacity)
        {
            /// <summary>
            /// Gets a value indicating whether the animation state contains a height transition.
            /// </summary>
            public bool HasAnimation => FromHeight > 0 || ToHeight > 0;
        }
    }
}