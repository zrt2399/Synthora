using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a tree-based navigation menu.
    /// </summary>
    public class TreeMenu : TreeView, ICustomKeyboardNavigation
    {
        private ContentPresenter? _selectedContentPresenter;

        /// <summary>
        /// Defines the <see cref="PaneIcon"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PaneIconProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneIcon));

        /// <summary>
        /// Defines the <see cref="PaneIconTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> PaneIconTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(PaneIconTemplate));

        /// <summary>
        /// Defines the <see cref="Header"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(Header));

        /// <summary>
        /// Defines the <see cref="HeaderTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(HeaderTemplate));

        /// <summary>
        /// Defines the <see cref="HeaderHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> HeaderHeightProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(HeaderHeight));

        /// <summary>
        /// Defines the <see cref="HeaderBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
            AvaloniaProperty.Register<TreeMenu, IBrush?>(nameof(HeaderBackground));

        /// <summary>
        /// Defines the <see cref="CompactPaneLength"/> property.
        /// </summary>
        public static readonly StyledProperty<double> CompactPaneLengthProperty =
            SplitView.CompactPaneLengthProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="DisplayMode"/> property.
        /// </summary>
        public static readonly StyledProperty<SplitViewDisplayMode> DisplayModeProperty =
            SplitView.DisplayModeProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="IsPaneOpen"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsPaneOpenProperty =
            SplitView.IsPaneOpenProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="OpenPaneLength"/> property.
        /// </summary>
        public static readonly StyledProperty<double> OpenPaneLengthProperty =
            SplitView.OpenPaneLengthProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="PaneBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> PaneBackgroundProperty =
            SplitView.PaneBackgroundProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="UseLightDismissOverlayMode"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> UseLightDismissOverlayModeProperty =
            SplitView.UseLightDismissOverlayModeProperty.AddOwner<TreeMenu>();

        /// <summary>
        /// Defines the <see cref="PaneHeader"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PaneHeaderProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneHeader));

        /// <summary>
        /// Defines the <see cref="PaneHeaderTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> PaneHeaderTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(PaneHeaderTemplate));

        /// <summary>
        /// Defines the <see cref="PaneFooter"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PaneFooterProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneFooter));

        /// <summary>
        /// Defines the <see cref="PaneFooterTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> PaneFooterTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(PaneFooterTemplate));

        /// <summary>
        /// Defines the <see cref="MenuMargin"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> MenuMarginProperty =
            AvaloniaProperty.Register<TreeMenu, Thickness>(nameof(MenuMargin));

        /// <summary>
        /// Defines the <see cref="SelectedItemTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(SelectedItemTemplate));

        /// <summary>
        /// Gets or sets the content displayed in the navigation pane toggle button.
        /// </summary>
        public object? PaneIcon
        {
            get => GetValue(PaneIconProperty);
            set => SetValue(PaneIconProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display <see cref="PaneIcon"/>.
        /// </summary>
        public IDataTemplate? PaneIconTemplate
        {
            get => GetValue(PaneIconTemplateProperty);
            set => SetValue(PaneIconTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed in the header bar.
        /// </summary>
        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display <see cref="Header"/>.
        /// </summary>
        public IDataTemplate? HeaderTemplate
        {
            get => GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the height of the header bar.
        /// </summary>
        public double HeaderHeight
        {
            get => GetValue(HeaderHeightProperty);
            set => SetValue(HeaderHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush of the header bar.
        /// </summary>
        public IBrush? HeaderBackground
        {
            get => GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the compact pane.
        /// </summary>
        public double CompactPaneLength
        {
            get => GetValue(CompactPaneLengthProperty);
            set => SetValue(CompactPaneLengthProperty, value);
        }

        /// <summary>
        /// Gets or sets how the pane is displayed.
        /// </summary>
        public SplitViewDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the pane is open.
        /// </summary>
        public bool IsPaneOpen
        {
            get => GetValue(IsPaneOpenProperty);
            set => SetValue(IsPaneOpenProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the open pane.
        /// </summary>
        public double OpenPaneLength
        {
            get => GetValue(OpenPaneLengthProperty);
            set => SetValue(OpenPaneLengthProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush of the pane.
        /// </summary>
        public IBrush? PaneBackground
        {
            get => GetValue(PaneBackgroundProperty);
            set => SetValue(PaneBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets whether light dismiss overlay mode is used.
        /// </summary>
        public bool UseLightDismissOverlayMode
        {
            get => GetValue(UseLightDismissOverlayModeProperty);
            set => SetValue(UseLightDismissOverlayModeProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed above the menu.
        /// </summary>
        public object? PaneHeader
        {
            get => GetValue(PaneHeaderProperty);
            set => SetValue(PaneHeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display <see cref="PaneHeader"/>.
        /// </summary>
        public IDataTemplate? PaneHeaderTemplate
        {
            get => GetValue(PaneHeaderTemplateProperty);
            set => SetValue(PaneHeaderTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed below the menu.
        /// </summary>
        public object? PaneFooter
        {
            get => GetValue(PaneFooterProperty);
            set => SetValue(PaneFooterProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display <see cref="PaneFooter"/>.
        /// </summary>
        public IDataTemplate? PaneFooterTemplate
        {
            get => GetValue(PaneFooterTemplateProperty);
            set => SetValue(PaneFooterTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the margin applied around the menu pane.
        /// </summary>
        public Thickness MenuMargin
        {
            get => GetValue(MenuMarginProperty);
            set => SetValue(MenuMarginProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to display the selected item content.
        /// </summary>
        public IDataTemplate? SelectedItemTemplate
        {
            get => GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
        }

        // Override and use the default KeyboardNavigation.
        (bool handled, IInputElement? next) ICustomKeyboardNavigation.GetNext(
            IInputElement element, NavigationDirection direction)
        {
            return (false, null);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _selectedContentPresenter = e.NameScope.Find<ContentPresenter>("PART_SelectedContentPresenter");
            UpdateSelectedContentPresenter();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SelectedItemProperty || change.Property == SelectedItemTemplateProperty)
            {
                UpdateSelectedContentPresenter();
            }
        }

        protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        {
            return new TreeMenuItem();
        }

        protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
        {
            return NeedsContainer<TreeMenuItem>(item, out recycleKey);
        }

        private void UpdateSelectedContentPresenter()
        {
            if (_selectedContentPresenter is null)
            {
                return;
            }

            if (SelectedItem is TreeMenuItem treeMenuItem)
            {
                _selectedContentPresenter.SetCurrentValue(TreeMenuItem.ContentProperty, treeMenuItem.Content);
                _selectedContentPresenter.SetCurrentValue(TreeMenuItem.ContentTemplateProperty, treeMenuItem.ContentTemplate ?? SelectedItemTemplate);
                return;
            }

            _selectedContentPresenter.SetCurrentValue(TreeMenuItem.ContentProperty, SelectedItem);
            _selectedContentPresenter.SetCurrentValue(TreeMenuItem.ContentTemplateProperty, SelectedItemTemplate);
        }
    }
}