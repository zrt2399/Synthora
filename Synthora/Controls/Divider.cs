using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a customizable divider control.
    /// </summary>
    public class Divider : ContentControl
    {
        private Grid? _dividerContainer;

        /// <summary>
        /// Defines the <see cref="LineThickness"/> property.
        /// This property determines the height when the orientation is horizontal, and the width when vertical.
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

        /// <summary>
        /// Defines the <see cref="RadiusX"/> property.
        /// Gets or sets the x-radius of the rounded corners of the divider.
        /// </summary>
        public static readonly StyledProperty<double> RadiusXProperty =
            Rectangle.RadiusXProperty.AddOwner<Divider>();

        /// <summary>
        /// Defines the <see cref="RadiusY"/> property.
        /// Gets or sets the y-radius of the rounded corners of the divider.
        /// </summary>
        public static readonly StyledProperty<double> RadiusYProperty =
            Rectangle.RadiusYProperty.AddOwner<Divider>();

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

        /// <summary>
        /// Gets or sets the x-radius of the rounded corners of the divider.
        /// </summary>
        public double RadiusX
        {
            get => GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// Gets or sets the y-radius of the rounded corners of the divider.
        /// </summary>
        public double RadiusY
        {
            get => GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MinLineLengthProperty ||
                change.Property == HorizontalContentAlignmentProperty ||
                change.Property == VerticalContentAlignmentProperty ||
                change.Property == OrientationProperty)
            {
                SetGrid();
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _dividerContainer = e.NameScope.Find<Grid>("PART_DividerContainer");
            SetGrid();
        }

        private void SetGrid()
        {
            if (_dividerContainer == null)
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

            var pixelLength = new GridLength(MinLineLength, GridUnitType.Pixel);
            var firstSegment = isStartAligned ? pixelLength : GridLength.Star;
            var lastSegment = isEndAligned ? pixelLength : GridLength.Star;

            _dividerContainer.ColumnDefinitions.Clear();
            _dividerContainer.ColumnDefinitions.Add(new ColumnDefinition(firstSegment));
            _dividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            _dividerContainer.ColumnDefinitions.Add(new ColumnDefinition(lastSegment));
        }
    }
}