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
        /// Defines the <see cref="PaneHeader"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PaneHeaderProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneHeader));

        /// <summary>
        /// Defines the <see cref="PaneFooter"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PaneFooterProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneFooter));

        /// <summary>
        /// Defines the <see cref="MenuMargin"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> MenuMarginProperty =
            AvaloniaProperty.Register<TreeMenu, Thickness>(nameof(MenuMargin));

        /// <summary>
        /// Defines the <see cref="MenuWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> MenuWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuWidth), 300d);

        /// <summary>
        /// Defines the <see cref="MenuMinWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> MenuMinWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuMinWidth), 180d);

        /// <summary>
        /// Defines the <see cref="MenuMaxWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> MenuMaxWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuMaxWidth), 480d);

        /// <summary>
        /// Defines the <see cref="MenuBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> MenuBackgroundProperty =
            AvaloniaProperty.Register<TreeMenu, IBrush?>(nameof(MenuBackground));

        /// <summary>
        /// Defines the <see cref="GridSplitterWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> GridSplitterWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(GridSplitterWidth), 1d);

        /// <summary>
        /// Defines the <see cref="SelectedItemTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(SelectedItemTemplate));

        /// <summary>
        /// Gets or sets the content displayed above the menu.
        /// </summary>
        public object? PaneHeader
        {
            get => GetValue(PaneHeaderProperty);
            set => SetValue(PaneHeaderProperty, value);
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
        /// Gets or sets the margin applied around the menu pane.
        /// </summary>
        public Thickness MenuMargin
        {
            get => GetValue(MenuMarginProperty);
            set => SetValue(MenuMarginProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the menu pane.
        /// </summary>
        public double MenuWidth
        {
            get => GetValue(MenuWidthProperty);
            set => SetValue(MenuWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum width of the menu pane.
        /// </summary>
        public double MenuMinWidth
        {
            get => GetValue(MenuMinWidthProperty);
            set => SetValue(MenuMinWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum width of the menu pane.
        /// </summary>
        public double MenuMaxWidth
        {
            get => GetValue(MenuMaxWidthProperty);
            set => SetValue(MenuMaxWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush of the menu pane.
        /// </summary>
        public IBrush? MenuBackground
        {
            get => GetValue(MenuBackgroundProperty);
            set => SetValue(MenuBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the grid splitter between the menu and content.
        /// </summary>
        public double GridSplitterWidth
        {
            get => GetValue(GridSplitterWidthProperty);
            set => SetValue(GridSplitterWidthProperty, value);
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
                _selectedContentPresenter.Content = treeMenuItem.Content;
                _selectedContentPresenter.ContentTemplate = treeMenuItem.ContentTemplate ?? SelectedItemTemplate;
                return;
            }

            _selectedContentPresenter.Content = SelectedItem;
            _selectedContentPresenter.ContentTemplate = SelectedItemTemplate;
        }
    }
}