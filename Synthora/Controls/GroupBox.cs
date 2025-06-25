using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class GroupBox : HeaderedContentControl
    { 
        public static readonly StyledProperty<Thickness> HeaderPaddingProperty =
            AvaloniaProperty.Register<GroupBox, Thickness>(nameof(HeaderPadding));

        public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
            AvaloniaProperty.Register<GroupBox, IBrush?>(nameof(HeaderBackground));

        public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
            AvaloniaProperty.Register<GroupBox, BoxShadows>(nameof(BoxShadow));

        public static readonly StyledProperty<double> HeaderDividerThicknessProperty =
            AvaloniaProperty.Register<GroupBox, double>(nameof(HeaderDividerThickness));

        public static readonly StyledProperty<Dock> HeaderPlacementProperty =
            AvaloniaProperty.Register<GroupBox, Dock>(nameof(HeaderPlacement));

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

        public double HeaderDividerThickness
        {
            get => GetValue(HeaderDividerThicknessProperty);
            set => SetValue(HeaderDividerThicknessProperty, value);
        }

        public Dock HeaderPlacement
        {
            get => GetValue(HeaderPlacementProperty);
            set => SetValue(HeaderPlacementProperty, value);
        }
    }
}