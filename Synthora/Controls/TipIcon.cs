using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Synthora.Messaging;

namespace Synthora.Controls
{
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
        public static SolidColorBrush IconForeground { get; } = new SolidColorBrush(Color.FromRgb(245, 245, 245));
        public static SolidColorBrush InformationBackground { get; } = new SolidColorBrush(Color.FromRgb(92, 138, 206));
        public static SolidColorBrush QuestionBackground { get; } = new SolidColorBrush(Color.FromRgb(60, 140, 240));
        public static SolidColorBrush SuccessBackground { get; } = new SolidColorBrush(Color.FromRgb(110, 190, 40));
        public static SolidColorBrush WarningBackground { get; } = new SolidColorBrush(Color.FromRgb(220, 155, 40));
        public static SolidColorBrush ErrorBackground { get; } = new SolidColorBrush(Color.FromRgb(230, 80, 80));

        static StatusIcon()
        {
            AffectsRender<IconBase>(IconTypeProperty);
            HeightProperty.OverrideDefaultValue<StatusIcon>(20);
            WidthProperty.OverrideDefaultValue<StatusIcon>(20);
            UseLayoutRoundingProperty.OverrideDefaultValue<StatusIcon>(false);
        }

        private double ActualHeight => Bounds.Height;
        private double ActualWidth => Bounds.Width;

        public override void Render(DrawingContext drawingContext)
        {
            if (IconType == IconType.Information)
            {
                drawingContext.DrawEllipse(InformationBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var exclamationPen = new Pen(IconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 4), new Point(10, 6));
                drawingContext.DrawLine(exclamationPen, new Point(10, 8), new Point(10, 16));
            }
            else if (IconType == IconType.Question)
            {
                drawingContext.DrawEllipse(QuestionBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(ActualWidth * 0.30, ActualHeight * 0.40), false);
                    ctx.CubicBezierTo(new Point(ActualWidth * 0.40, ActualHeight * 0.1), new Point(ActualWidth * 0.90, ActualHeight * 0.2), new Point(ActualWidth * 0.50, ActualHeight * 0.5), true);
                    ctx.LineTo(new Point(ActualWidth * 0.50, ActualHeight * 0.60), true);
                }
                var markPen = new Pen(IconForeground, 2);
                drawingContext.DrawGeometry(null, markPen, geometry);

                var dotRadius = 1.4;
                var dotCenter = new Point(ActualWidth * 0.50, ActualHeight * 0.75);
                drawingContext.DrawEllipse(IconForeground, null, dotCenter, dotRadius, dotRadius);
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

                var pen = new Pen(SuccessBackground, 4);
                drawingContext.DrawGeometry(SuccessBackground, pen, geometry);
            }
            else if (IconType == IconType.Warning)
            {
                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(10, 0), true);
                    ctx.LineTo(new Point(20, 20));
                    ctx.LineTo(new Point(0, 20));
                    ctx.EndFigure(true);
                }

                var pen = new Pen(WarningBackground, 2);
                pen.LineJoin = PenLineJoin.Bevel;

                drawingContext.DrawGeometry(WarningBackground, pen, geometry);
                var exclamationPen = new Pen(IconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 5), new Point(10, 13));
                drawingContext.DrawLine(exclamationPen, new Point(10, 15), new Point(10, 17));
            }
            else if (IconType == IconType.Error)
            {
                drawingContext.DrawEllipse(ErrorBackground, null, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2, ActualWidth / 2);

                var exclamationPen = new Pen(IconForeground, 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 4), new Point(10, 12));
                drawingContext.DrawLine(exclamationPen, new Point(10, 14), new Point(10, 16));
            }
        }
    }
}