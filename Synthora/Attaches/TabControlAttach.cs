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

        public static HorizontalAlignment GetHorizontalHeaderAlignment(Control obj) => obj.GetValue(HorizontalHeaderAlignmentProperty);
        public static void SetHorizontalHeaderAlignment(Control obj, HorizontalAlignment value) => obj.SetValue(HorizontalHeaderAlignmentProperty, value);

        public static VerticalAlignment GetVerticalHeaderAlignment(Control obj) => obj.GetValue(VerticalHeaderAlignmentProperty);
        public static void SetVerticalHeaderAlignment(Control obj, VerticalAlignment value) => obj.SetValue(VerticalHeaderAlignmentProperty, value);

        public static bool GetShowHeaderSeparator(Control obj) => obj.GetValue(ShowHeaderSeparatorProperty);
        public static void SetShowHeaderSeparator(Control obj, bool value) => obj.SetValue(ShowHeaderSeparatorProperty, value);
    }
}