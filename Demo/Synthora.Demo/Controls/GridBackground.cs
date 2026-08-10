using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Demo.Controls
{
    public class GridBackground : Control
    {
        public static readonly StyledProperty<IBrush?> GridBrushProperty =
            AvaloniaProperty.Register<GridBackground, IBrush?>(nameof(GridBrush));

        public static readonly StyledProperty<double> GridSizeProperty =
            AvaloniaProperty.Register<GridBackground, double>(
                nameof(GridSize), defaultValue: 80d, coerce: (_, value) => double.IsFinite(value) && value > 0 ? value : 80d);

        public static readonly StyledProperty<double> LineThicknessProperty =
            AvaloniaProperty.Register<GridBackground, double>(
                nameof(LineThickness), defaultValue: 1d, coerce: (_, value) => double.IsFinite(value) && value > 0 ? value : 1d);

        public static readonly StyledProperty<bool> DrawBorderProperty =
            AvaloniaProperty.Register<GridBackground, bool>(nameof(DrawBorder));

        public IBrush? GridBrush
        {
            get => GetValue(GridBrushProperty);
            set => SetValue(GridBrushProperty, value);
        }

        public double GridSize
        {
            get => GetValue(GridSizeProperty);
            set => SetValue(GridSizeProperty, value);
        }

        public double LineThickness
        {
            get => GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        public bool DrawBorder
        {
            get => GetValue(DrawBorderProperty);
            set => SetValue(DrawBorderProperty, value);
        }

        static GridBackground()
        {
            AffectsRender<GridBackground>(GridBrushProperty, GridSizeProperty, LineThicknessProperty, DrawBorderProperty);
        }

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            var gridSize = GridSize;
            var lineThickness = LineThickness;
            var drawBorder = DrawBorder;
            if (GridBrush is not IBrush gridBrush)
            {
                return;
            }

            var pen = new Pen(gridBrush, lineThickness);
            for (var x = gridSize; x < Bounds.Width; x += gridSize)
            {
                drawingContext.DrawLine(pen, new Point(x, 0), new Point(x, Bounds.Height));
            }

            for (var y = gridSize; y < Bounds.Height; y += gridSize)
            {
                drawingContext.DrawLine(pen, new Point(0, y), new Point(Bounds.Width, y));
            }

            if (drawBorder)
            {
                drawingContext.DrawRectangle(null, pen, new Rect(Bounds.Size));
            }
        }
    }
}