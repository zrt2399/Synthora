using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class GlowBackground : Control
    {
        private static int _nextGlowSeed = Random.Shared.Next();

        private static readonly Color[] DefaultGlowColors =
        [
            Color.Parse("#40007DFF"),
            Color.Parse("#40A855F7"),
            Color.Parse("#4099FBF3"),
            Color.Parse("#405B8CFF"),
            Color.Parse("#407C4DFF"),
            Color.Parse("#40DB75FF"),
            Color.Parse("#4054C4FF"),
            Color.Parse("#409A74FF"),
            Color.Parse("#40505CFF"),
            Color.Parse("#40A684FF"),
            Color.Parse("#407A68FF"),
            Color.Parse("#405FA8F7"),
        ];

        private readonly List<Glow> _glows = [];
        private Size _glowCacheSize;

        public static readonly StyledProperty<IBrush?> GridBrushProperty =
            AvaloniaProperty.Register<GlowBackground, IBrush?>(nameof(GridBrush));

        public static readonly StyledProperty<double> GridSizeProperty =
            AvaloniaProperty.Register<GlowBackground, double>(
                nameof(GridSize), defaultValue: 100d, coerce: (_, value) => double.IsFinite(value) && value > 0 ? value : 100d);

        public static readonly StyledProperty<double> LineThicknessProperty =
            AvaloniaProperty.Register<GlowBackground, double>(
                nameof(LineThickness), defaultValue: 1d, coerce: (_, value) => double.IsFinite(value) && value > 0 ? value : 1d);

        public static readonly StyledProperty<bool> DrawBorderProperty =
            AvaloniaProperty.Register<GlowBackground, bool>(nameof(DrawBorder));

        public static readonly StyledProperty<int> GlowCountProperty =
            AvaloniaProperty.Register<GlowBackground, int>(
                nameof(GlowCount), defaultValue: 4, coerce: (_, value) => Math.Max(0, value));

        public static readonly StyledProperty<double> GlowSizeProperty =
            AvaloniaProperty.Register<GlowBackground, double>(
                nameof(GlowSize), defaultValue: 500d, coerce: (_, value) => double.IsFinite(value) && value > 0 ? value : 600d);

        public static readonly StyledProperty<bool> AllowGlowOutsideBoundsProperty =
            AvaloniaProperty.Register<GlowBackground, bool>(nameof(AllowGlowOutsideBounds));

        public static readonly StyledProperty<int> GlowSeedProperty =
            AvaloniaProperty.Register<GlowBackground, int>(nameof(GlowSeed));

        public static readonly StyledProperty<IReadOnlyList<Color>?> GlowColorsProperty =
            AvaloniaProperty.Register<GlowBackground, IReadOnlyList<Color>?>(nameof(GlowColors), defaultValue: DefaultGlowColors);

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

        public int GlowCount
        {
            get => GetValue(GlowCountProperty);
            set => SetValue(GlowCountProperty, value);
        }

        public double GlowSize
        {
            get => GetValue(GlowSizeProperty);
            set => SetValue(GlowSizeProperty, value);
        }

        public bool AllowGlowOutsideBounds
        {
            get => GetValue(AllowGlowOutsideBoundsProperty);
            set => SetValue(AllowGlowOutsideBoundsProperty, value);
        }

        public int GlowSeed
        {
            get => GetValue(GlowSeedProperty);
            set => SetValue(GlowSeedProperty, value);
        }

        public IReadOnlyList<Color>? GlowColors
        {
            get => GetValue(GlowColorsProperty);
            set => SetValue(GlowColorsProperty, value);
        }

        public GlowBackground()
        {
            SetCurrentValue(GlowSeedProperty, Interlocked.Increment(ref _nextGlowSeed));
        }

        static GlowBackground()
        {
            AffectsRender<GlowBackground>(GridBrushProperty, GridSizeProperty, LineThicknessProperty, DrawBorderProperty,
                GlowCountProperty, GlowSizeProperty, AllowGlowOutsideBoundsProperty, GlowSeedProperty, GlowColorsProperty);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == GlowCountProperty ||
                change.Property == GlowSizeProperty ||
                change.Property == AllowGlowOutsideBoundsProperty ||
                change.Property == GlowSeedProperty ||
                change.Property == GlowColorsProperty)
            {
                ClearGlowCache();
            }
        }

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            if (GridBrush is IBrush gridBrush)
            {
                var gridSize = GridSize;
                var lineThickness = LineThickness;
                var drawBorder = DrawBorder;

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

            DrawGlows(drawingContext);
        }

        private void DrawGlows(DrawingContext drawingContext)
        {
            var colors = GlowColors;
            if (GlowCount == 0 || colors is null || colors.Count == 0 || Bounds.Width <= 0 || Bounds.Height <= 0)
            {
                return;
            }

            var glowCount = GlowCount;
            if (_glowCacheSize != Bounds.Size || _glows.Count != glowCount)
            {
                CreateGlows(colors, glowCount);
            }

            foreach (var glow in _glows)
            {
                drawingContext.DrawEllipse(glow.Brush, null, glow.Center, glow.Radius, glow.Radius);
            }
        }

        private void CreateGlows(IReadOnlyList<Color> colors, int glowCount)
        {
            _glows.Clear();
            _glowCacheSize = Bounds.Size;

            var diameter = Math.Min(GlowSize, Math.Min(Bounds.Width, Bounds.Height));
            var radius = diameter / 2;
            var random = new Random(GlowSeed);
            var colorIndexes = glowCount <= colors.Count ? CreateShuffledIndexes(colors.Count, random) : null;
            var positionIndexes = CreateShuffledIndexes(glowCount, random);
            var columnCount = (int)Math.Ceiling(Math.Sqrt(glowCount));
            var rowCount = (int)Math.Ceiling((double)glowCount / columnCount);
            var allowGlowOutsideBounds = AllowGlowOutsideBounds;
            var left = allowGlowOutsideBounds ? 0 : radius;
            var top = allowGlowOutsideBounds ? 0 : radius;
            var width = Bounds.Width - (allowGlowOutsideBounds ? 0 : diameter);
            var height = Bounds.Height - (allowGlowOutsideBounds ? 0 : diameter);
            for (var i = 0; i < glowCount; i++)
            {
                var positionIndex = positionIndexes[i];
                var column = positionIndex % columnCount;
                var row = positionIndex / columnCount;
                var x = left + (column + random.NextDouble()) * width / columnCount;
                var y = top + (row + random.NextDouble()) * height / rowCount;
                var color = colorIndexes is null ? colors[random.Next(colors.Count)] : colors[colorIndexes[i]];
                var brush = new RadialGradientBrush
                {
                    GradientStops = new GradientStops
                    {
                        new GradientStop(color, 0),
                        new GradientStop(Colors.Transparent, 1),
                    },
                };

                _glows.Add(new Glow(new Point(x, y), radius, brush));
            }
        }

        private static List<int> CreateShuffledIndexes(int count, Random random)
        {
            var indexes = new List<int>(count);
            for (var i = 0; i < count; i++)
            {
                indexes.Add(i);
            }

            for (var i = indexes.Count - 1; i > 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                (indexes[i], indexes[swapIndex]) = (indexes[swapIndex], indexes[i]);
            }

            return indexes;
        }

        private void ClearGlowCache()
        {
            _glows.Clear();
            _glowCacheSize = default;
        }

        private sealed record Glow(Point Center, double Radius, IBrush Brush);
    }
}
