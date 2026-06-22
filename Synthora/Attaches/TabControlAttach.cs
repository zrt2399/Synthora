using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class TabControlAttach
    {
        public static readonly AttachedProperty<HorizontalAlignment> HorizontalHeaderAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, HorizontalAlignment>("HorizontalHeaderAlignment", defaultValue: HorizontalAlignment.Left);

        public static readonly AttachedProperty<VerticalAlignment> VerticalHeaderAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, VerticalAlignment>("VerticalHeaderAlignment", defaultValue: VerticalAlignment.Top);

        public static readonly AttachedProperty<bool> ShowHeaderSeparatorProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, bool>("ShowHeaderSeparator", defaultValue: true);

        public static readonly AttachedProperty<CornerRadius> HeaderCornerRadiusProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, CornerRadius>("HeaderCornerRadius");

        public static readonly AttachedProperty<Thickness> HeaderBorderThicknessProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, Thickness>("HeaderBorderThickness");

        public static readonly AttachedProperty<Thickness> HeaderPaddingProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, Control, Thickness>("HeaderPadding");

        public static HorizontalAlignment GetHorizontalHeaderAlignment(Control obj) => obj.GetValue(HorizontalHeaderAlignmentProperty);
        public static void SetHorizontalHeaderAlignment(Control obj, HorizontalAlignment value) => obj.SetValue(HorizontalHeaderAlignmentProperty, value);

        public static VerticalAlignment GetVerticalHeaderAlignment(Control obj) => obj.GetValue(VerticalHeaderAlignmentProperty);
        public static void SetVerticalHeaderAlignment(Control obj, VerticalAlignment value) => obj.SetValue(VerticalHeaderAlignmentProperty, value);

        public static bool GetShowHeaderSeparator(Control obj) => obj.GetValue(ShowHeaderSeparatorProperty);
        public static void SetShowHeaderSeparator(Control obj, bool value) => obj.SetValue(ShowHeaderSeparatorProperty, value);

        public static CornerRadius GetHeaderCornerRadius(Control obj) => obj.GetValue(HeaderCornerRadiusProperty);
        public static void SetHeaderCornerRadius(Control obj, CornerRadius value) => obj.SetValue(HeaderCornerRadiusProperty, value);

        public static Thickness GetHeaderBorderThickness(Control obj) => obj.GetValue(HeaderBorderThicknessProperty);
        public static void SetHeaderBorderThickness(Control obj, Thickness value) => obj.SetValue(HeaderBorderThicknessProperty, value);

        public static Thickness GetHeaderPadding(Control obj) => obj.GetValue(HeaderPaddingProperty);
        public static void SetHeaderPadding(Control obj, Thickness value) => obj.SetValue(HeaderPaddingProperty, value);
    }
}