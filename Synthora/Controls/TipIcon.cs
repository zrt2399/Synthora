using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Synthora.Resources;

namespace Synthora.Controls
{
    /// <summary>
    /// Defines the type of icon to display in a message or dialog.
    /// </summary>
    public enum IconType
    {
        None,
        Information,
        Question,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// A base control for displaying an icon based on the specified <see cref="IconType"/>.
    /// </summary>
    public abstract class IconBase : TemplatedControl
    {
        /// <summary>
        /// Defines the <see cref="IconType"/> property.
        /// Determines which type of icon (information, warning, error, etc.) to render.
        /// </summary>
        public static readonly StyledProperty<IconType> IconTypeProperty =
            AvaloniaProperty.Register<IconBase, IconType>(nameof(IconType));

        /// <summary>
        /// Gets or sets the type of icon to display.
        /// </summary>
        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }
    }

    /// <summary>
    /// A specialized icon control that inherits from <see cref="IconBase"/>.
    /// Can be used to render tip-specific icons (e.g., in message tips).
    /// </summary>
    public class TipIcon : IconBase
    {

    }

    internal class StatusIcon : IconBase
    {
        static StatusIcon()
        {
            AffectsRender<IconBase>(IconTypeProperty);
            HeightProperty.OverrideDefaultValue<StatusIcon>(20);
            WidthProperty.OverrideDefaultValue<StatusIcon>(20);
            UseLayoutRoundingProperty.OverrideDefaultValue<StatusIcon>(false);
        }

        private static readonly IImmutableSolidColorBrush _iconForeground = SynthoraBrushes.IconForeground;
        private static readonly IImmutableSolidColorBrush _informationBackground = SynthoraBrushes.InformationBrush;
        private static readonly IImmutableSolidColorBrush _questionBackground = SynthoraBrushes.QuestionBrush;
        private static readonly IImmutableSolidColorBrush _successBackground = SynthoraBrushes.SuccessBrush;
        private static readonly IImmutableSolidColorBrush _warningBackground = SynthoraBrushes.WarningBrush;
        private static readonly IImmutableSolidColorBrush _errorBackground = SynthoraBrushes.ErrorBrush;

        private double ActualHeight => Bounds.Height;
        private double ActualWidth => Bounds.Width;

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            if (IconType == IconType.Information)
            {
                drawingContext.DrawEllipse(_informationBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var exclamationPen = new Pen(_iconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 4), new Point(10, 6));
                drawingContext.DrawLine(exclamationPen, new Point(10, 8), new Point(10, 16));
            }
            else if (IconType == IconType.Question)
            {
                drawingContext.DrawEllipse(_questionBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(ActualWidth * 0.30, ActualHeight * 0.40), false);
                    ctx.CubicBezierTo(new Point(ActualWidth * 0.40, ActualHeight * 0.1), new Point(ActualWidth * 0.90, ActualHeight * 0.2), new Point(ActualWidth * 0.50, ActualHeight * 0.5));
                    ctx.LineTo(new Point(ActualWidth * 0.50, ActualHeight * 0.60));
                }
                var markPen = new Pen(_iconForeground, 2);
                drawingContext.DrawGeometry(null, markPen, geometry);

                var dotRadius = 1.4;
                var dotCenter = new Point(ActualWidth * 0.50, ActualHeight * 0.75);
                drawingContext.DrawEllipse(_iconForeground, null, dotCenter, dotRadius, dotRadius);
            }
            else if (IconType == IconType.Success)
            {
                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(1.4, 10), false);
                    ctx.LineTo(new Point(8, 16));
                    ctx.LineTo(new Point(18.4, 2));
                }

                var pen = new Pen(_successBackground, 4);
                drawingContext.DrawGeometry(_successBackground, pen, geometry);
            }
            else if (IconType == IconType.Warning)
            {
                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(9, 3));
                    ctx.CubicBezierTo(new Point(9, 2), new Point(11, 2), new Point(11, 3));
                    ctx.LineTo(new Point(18, 17));
                    ctx.CubicBezierTo(new Point(19, 18), new Point(18, 19), new Point(17, 19));
                    ctx.LineTo(new Point(3, 19));
                    ctx.CubicBezierTo(new Point(2, 19), new Point(1, 18), new Point(2, 17));
                    ctx.EndFigure(true);
                }

                var pen = new Pen(_warningBackground, 2, lineJoin: PenLineJoin.Round);

                drawingContext.DrawGeometry(_warningBackground, pen, geometry);
                var exclamationPen = new Pen(_iconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 5), new Point(10, 13));
                drawingContext.DrawLine(exclamationPen, new Point(10, 15), new Point(10, 17));
            }
            else if (IconType == IconType.Error)
            {
                drawingContext.DrawEllipse(_errorBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var exclamationPen = new Pen(_iconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 4), new Point(10, 12));
                drawingContext.DrawLine(exclamationPen, new Point(10, 14), new Point(10, 16));
            }
        }
    }
}