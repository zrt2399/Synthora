using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class GroupBox : HeaderedContentControl
    {
        static GroupBox()
        {
            AffectsRender<DropShadowChrome>(BoxShadowProperty);
        }

        public static readonly StyledProperty<Thickness> HeaderPaddingProperty =
            AvaloniaProperty.Register<GroupBox, Thickness>(nameof(HeaderPadding));

        public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
            AvaloniaProperty.Register<GroupBox, IBrush?>(nameof(HeaderBackground));

        public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
            AvaloniaProperty.Register<GroupBox, BoxShadows>(nameof(BoxShadow));

        public static readonly StyledProperty<double> HeaderDividerHeightProperty =
            AvaloniaProperty.Register<GroupBox, double>(nameof(HeaderDividerHeight));

        public Thickness HeaderPadding
        {
            get => GetValue(HeaderPaddingProperty);
            set => SetValue(HeaderPaddingProperty, value);
        }

        public IBrush? HeaderBackground
        {
            get => GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }

        public double HeaderDividerHeight
        {
            get => GetValue(HeaderDividerHeightProperty);
            set => SetValue(HeaderDividerHeightProperty, value);
        }
    }
}