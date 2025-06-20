using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class TabControlAttach
    {
        public static readonly AttachedProperty<HorizontalAlignment> HorizontalHeaderAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, TabControl, HorizontalAlignment>("HorizontalHeaderAlignment");

        public static readonly AttachedProperty<VerticalAlignment> VerticalHeaderAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TabControlAttach, TabControl, VerticalAlignment>("VerticalHeaderAlignment");

        public static HorizontalAlignment GetHorizontalHeaderAlignment(TabControl obj) => obj.GetValue(HorizontalHeaderAlignmentProperty);
        public static void SetHorizontalHeaderAlignment(TabControl obj, HorizontalAlignment value) => obj.SetValue(HorizontalHeaderAlignmentProperty, value);
        
        public static VerticalAlignment GetVerticalHeaderAlignment(TabControl obj) => obj.GetValue(VerticalHeaderAlignmentProperty);
        public static void SetVerticalHeaderAlignment(TabControl obj, VerticalAlignment value) => obj.SetValue(VerticalHeaderAlignmentProperty, value);
    }
}