using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Synthora.Controls
{
    public class ProgressRing : Control
    {
        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<ProgressRing, bool>(nameof(IsActive), true);

        public static readonly StyledProperty<double> StrokeThicknessProperty =
            AvaloniaProperty.Register<ProgressRing, double>(nameof(StrokeThickness));

        public static readonly StyledProperty<IBrush?> ForegroundProperty =
            AvaloniaProperty.Register<ProgressRing, IBrush?>(nameof(Foreground));

        private readonly DispatcherTimer _animationTimer;
        private long _lastTimestamp;
        private double _elapsedMilliseconds;

        static ProgressRing()
        {
            AffectsRender<ProgressRing>(IsActiveProperty, StrokeThicknessProperty, ForegroundProperty);
            AffectsMeasure<ProgressRing>(StrokeThicknessProperty);
            IsActiveProperty.Changed.AddClassHandler<ProgressRing>((s, e) => s.UpdateAnimationState());
        }

        public ProgressRing()
        {
            _animationTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(16), DispatcherPriority.Render, OnAnimationTick);
        }

        public bool IsActive
        {
            get => GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public IBrush? Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UpdateAnimationState();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _animationTimer.Stop();
            _lastTimestamp = 0;
            base.OnDetachedFromVisualTree(e);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == IsVisibleProperty || change.Property.Name == nameof(IsEffectivelyVisible))
            {
                UpdateAnimationState();
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Foreground is not { } foreground)
            {
                return;
            }

            var diameter = Math.Min(Bounds.Width, Bounds.Height);
            var strokeThickness = Math.Min(StrokeThickness, diameter / 2);
            if (diameter <= 0 || strokeThickness <= 0)
            {
                return;
            }

            var radius = (diameter - strokeThickness) / 2;
            var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
            var cycleProgress = (_elapsedMilliseconds % 1300) / 1300;
            var sweep = 70 + (Math.Sin(cycleProgress * Math.PI * 2 - Math.PI / 2) + 1) * 90;
            var startAngle = (_elapsedMilliseconds * 0.36) % 360;
            var endAngle = startAngle + sweep;
            var geometry = CreateArcGeometry(center, radius, startAngle, endAngle);
            var pen = new Pen(foreground, strokeThickness, null, PenLineCap.Round, PenLineJoin.Round);

            context.DrawGeometry(null, pen, geometry);
        }

        private static StreamGeometry CreateArcGeometry(Point center, double radius, double startAngle, double endAngle)
        {
            var start = GetPointOnCircle(center, radius, startAngle);
            var end = GetPointOnCircle(center, radius, endAngle);
            var geometry = new StreamGeometry();

            using (var context = geometry.Open())
            {
                context.BeginFigure(start, false);
                context.ArcTo(end, new Size(radius, radius), 0, endAngle - startAngle > 180, SweepDirection.Clockwise);
            }

            return geometry;
        }

        private static Point GetPointOnCircle(Point center, double radius, double angle)
        {
            var radians = (angle - 90) * Math.PI / 180;
            return new Point(center.X + radius * Math.Cos(radians), center.Y + radius * Math.Sin(radians));
        }

        private void OnAnimationTick(object? sender, EventArgs e)
        {
            var now = TimeProvider.System.GetTimestamp();

            if (_lastTimestamp != 0)
            {
                _elapsedMilliseconds += TimeProvider.System.GetElapsedTime(_lastTimestamp, now).TotalMilliseconds;
            }

            _lastTimestamp = now;
            InvalidateVisual();
        }

        private void UpdateAnimationState()
        {
            if (IsActive && VisualRoot is not null && IsEffectivelyVisible)
            {
                if (!_animationTimer.IsEnabled)
                {
                    _lastTimestamp = 0;
                    _animationTimer.Start();
                }

                InvalidateVisual();
            }
            else
            {
                _animationTimer.Stop();
                _lastTimestamp = 0;
                InvalidateVisual();
            }
        }
    }
}