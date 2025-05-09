using Avalonia;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    public class EmptyBox : TemplatedControl
    {
        public static readonly StyledProperty<double> GlyphWidthProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(GlyphWidth), 60d);

        public static readonly StyledProperty<double> GlyphHeightProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(GlyphHeight), 40d);

        public double GlyphWidth
        {
            get => GetValue(GlyphWidthProperty);
            set => SetValue(GlyphWidthProperty, value);
        }

        public double GlyphHeight
        {
            get => GetValue(GlyphHeightProperty);
            set => SetValue(GlyphHeightProperty, value);
        }
    }
}