using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// A custom control that displays a divider with content.
    /// </summary>
    [PseudoClasses(pcNoContent)]
    public class Divider : ContentControl
    {
        private const string pcNoContent = ":no-content";
        private Grid? PART_DividerContainer;

        /// <summary>
        /// Gets or sets the thickness of the divider line,
        /// which applies to height in horizontal layout and width in vertical layout.
        /// </summary>
        public static readonly StyledProperty<double> LineThicknessProperty =
            AvaloniaProperty.Register<Divider, double>(nameof(LineThickness), 1d);

        /// <summary>
        /// Defines the <see cref="LineBrush"/> property.
        /// Determines the brush used to draw the divider.
        /// </summary>
        public static readonly StyledProperty<IBrush?> LineBrushProperty =
            AvaloniaProperty.Register<Divider, IBrush?>(nameof(LineBrush));

        /// <summary>
        /// Defines the <see cref="MinLineLength"/> property.
        /// Represents the minimum length of the shorter divider (left or right),
        /// while the opposite side stretches to fill the remaining space.
        /// </summary>
        public static readonly StyledProperty<double> MinLineLengthProperty =
            AvaloniaProperty.Register<Divider, double>(nameof(MinLineLength), 40d);

        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// Determines the layout direction of the divider: horizontal or vertical.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            AvaloniaProperty.Register<Divider, Orientation>(nameof(Orientation));

        static Divider()
        {
            MinLineLengthProperty.Changed.AddClassHandler<Divider, double>((s, e) => s.SetGrid());
            HorizontalContentAlignmentProperty.Changed.AddClassHandler<Divider, HorizontalAlignment>((s, e) => s.SetGrid());
            VerticalContentAlignmentProperty.Changed.AddClassHandler<Divider, VerticalAlignment>((s, e) => s.SetGrid());
            OrientationProperty.Changed.AddClassHandler<Divider, Orientation>((s, e) => s.SetGrid());
        }

        /// <summary>
        /// Gets or sets the height of the divider.
        /// </summary>
        public double LineThickness
        {
            get => GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush that paints the divider.
        /// </summary>
        public IBrush? LineBrush
        {
            get => GetValue(LineBrushProperty);
            set => SetValue(LineBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the layout orientation of the divider.
        /// </summary>
        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum length of the shorter divider (either left or right).
        /// </summary>
        public double MinLineLength
        {
            get => GetValue(MinLineLengthProperty);
            set => SetValue(MinLineLengthProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == ContentProperty)
            {
                UpdatePseudoClasses();
            }

            base.OnPropertyChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PART_DividerContainer = e.NameScope.Find<Grid>(nameof(PART_DividerContainer));
            SetGrid();
            UpdatePseudoClasses();
        }

        private void UpdatePseudoClasses()
        {
            PseudoClasses.Set(pcNoContent, Content == null);
        }

        private void SetGrid()
        {
            if (PART_DividerContainer == null)
            {
                return;
            }

            var isHorizontal = Orientation == Orientation.Horizontal;

            var isStartAligned = isHorizontal
                ? HorizontalContentAlignment == HorizontalAlignment.Left
                : VerticalContentAlignment == VerticalAlignment.Top;

            var isEndAligned = isHorizontal
                ? HorizontalContentAlignment == HorizontalAlignment.Right
                : VerticalContentAlignment == VerticalAlignment.Bottom;

            GridLength pixelLength = new GridLength(MinLineLength, GridUnitType.Pixel); 
            GridLength firstSegment = isStartAligned ? pixelLength : GridLength.Star;
            GridLength lastSegment = isEndAligned ? pixelLength : GridLength.Star;

            PART_DividerContainer.ColumnDefinitions.Clear();
            PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(firstSegment));
            PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(lastSegment));
        }
    }
}