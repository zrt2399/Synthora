using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class TreeMenu : TreeView
    {
        private ContentPresenter? _selectedContentPresenter;

        public static readonly StyledProperty<object?> PaneHeaderProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneHeader));

        public static readonly StyledProperty<object?> PaneFooterProperty =
            AvaloniaProperty.Register<TreeMenu, object?>(nameof(PaneFooter));

        public static readonly StyledProperty<Thickness> MenuMarginProperty =
            AvaloniaProperty.Register<TreeMenu, Thickness>(nameof(MenuMargin));

        public static readonly StyledProperty<double> MenuWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuWidth), 300d);

        public static readonly StyledProperty<double> MenuMinWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuMinWidth), 200d);

        public static readonly StyledProperty<double> MenuMaxWidthProperty =
            AvaloniaProperty.Register<TreeMenu, double>(nameof(MenuMaxWidth), 500d);

        public static readonly StyledProperty<IBrush?> MenuBackgroundProperty =
            AvaloniaProperty.Register<TreeMenu, IBrush?>(nameof(MenuBackground));

        public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
            AvaloniaProperty.Register<TreeMenu, IDataTemplate?>(nameof(SelectedItemTemplate));

        public object? PaneHeader
        {
            get => GetValue(PaneHeaderProperty);
            set => SetValue(PaneHeaderProperty, value);
        }

        public object? PaneFooter
        {
            get => GetValue(PaneFooterProperty);
            set => SetValue(PaneFooterProperty, value);
        }

        public Thickness MenuMargin
        {
            get => GetValue(MenuMarginProperty);
            set => SetValue(MenuMarginProperty, value);
        }

        public double MenuWidth
        {
            get => GetValue(MenuWidthProperty);
            set => SetValue(MenuWidthProperty, value);
        }

        public double MenuMinWidth
        {
            get => GetValue(MenuMinWidthProperty);
            set => SetValue(MenuMinWidthProperty, value);
        }

        public double MenuMaxWidth
        {
            get => GetValue(MenuMaxWidthProperty);
            set => SetValue(MenuMaxWidthProperty, value);
        }

        public IBrush? MenuBackground
        {
            get => GetValue(MenuBackgroundProperty);
            set => SetValue(MenuBackgroundProperty, value);
        }

        public IDataTemplate? SelectedItemTemplate
        {
            get => GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
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