using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// A custom control that displays a divider line with content.
    /// </summary>
    public class LabelDivider : ContentControl
    {
        private Grid? PART_DividerContainer;

        /// <summary>
        /// Defines the <see cref="LineHeight"/> property.
        /// Represents the height of the divider line (in device-independent units).
        /// </summary>
        public static readonly StyledProperty<double> LineHeightProperty =
            AvaloniaProperty.Register<LabelDivider, double>(nameof(LineHeight), 1d);

        /// <summary>
        /// Defines the <see cref="LineBrush"/> property.
        /// Determines the brush used to draw the divider line.
        /// </summary>
        public static readonly StyledProperty<IBrush?> LineBrushProperty =
            AvaloniaProperty.Register<LabelDivider, IBrush?>(nameof(LineBrush));

        /// <summary>
        /// Gets or sets the minimum length of the shorter divider line (either left or right).
        /// The other side will stretch automatically to fill the remaining space.
        /// </summary>
        public static readonly StyledProperty<double> MinLineWidthProperty =
            AvaloniaProperty.Register<LabelDivider, double>(nameof(MinLineWidth), 40d);

        static LabelDivider()
        {
            MinLineWidthProperty.Changed.AddClassHandler<LabelDivider, double>((s, e) => s.SetGrid());
            HorizontalContentAlignmentProperty.Changed.AddClassHandler<LabelDivider, HorizontalAlignment>((s, e) => s.SetGrid());
        }

        /// <summary>
        /// Gets or sets the height of the divider line.
        /// </summary>
        public double LineHeight
        {
            get => GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush that paints the divider line.
        /// </summary>
        public IBrush? LineBrush
        {
            get => GetValue(LineBrushProperty);
            set => SetValue(LineBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum length of the shorter divider line (either left or right).
        /// </summary>
        public double MinLineWidth
        {
            get => GetValue(MinLineWidthProperty);
            set => SetValue(MinLineWidthProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PART_DividerContainer = e.NameScope.Find<Grid>(nameof(PART_DividerContainer));
            SetGrid();
        }

        private void SetGrid()
        {
            if (PART_DividerContainer == null)
            {
                return;
            }

            PART_DividerContainer.ColumnDefinitions.Clear();
            switch (HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(MinLineWidth, GridUnitType.Pixel));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    break;
                case HorizontalAlignment.Right:
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(MinLineWidth, GridUnitType.Pixel));
                    break;
                default:
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                    PART_DividerContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    break;
            }
        }
    }
}