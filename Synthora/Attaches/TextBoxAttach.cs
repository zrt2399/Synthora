using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class TextBoxAttach
    {
        public static readonly AttachedProperty<object?> HeaderProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, object?>("Header");

        public static readonly AttachedProperty<Dock> HeaderPlacementProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, Dock>("HeaderPlacement", Dock.Top);

        public static readonly AttachedProperty<HorizontalAlignment> HeaderHorizontalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, HorizontalAlignment>("HeaderHorizontalAlignment", HorizontalAlignment.Left);

        public static readonly AttachedProperty<VerticalAlignment> HeaderVerticalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, VerticalAlignment>("HeaderVerticalAlignment", VerticalAlignment.Center);

        public static readonly AttachedProperty<double> MinWidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("MinWidth");

        public static readonly AttachedProperty<double> WidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("Width", double.NaN);

        public static readonly AttachedProperty<double> MaxWidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("MaxWidth", double.PositiveInfinity);

        public static object? GetHeader(Control obj) => obj.GetValue(HeaderProperty);
        public static void SetHeader(Control obj, object? value) => obj.SetValue(HeaderProperty, value);

        public static Dock GetHeaderPlacement(Control obj) => obj.GetValue(HeaderPlacementProperty);
        public static void SetHeaderPlacement(Control obj, Dock value) => obj.SetValue(HeaderPlacementProperty, value);

        public static HorizontalAlignment GetHeaderHorizontalAlignment(Control obj) => obj.GetValue(HeaderHorizontalAlignmentProperty);
        public static void SetHeaderHorizontalAlignment(Control obj, HorizontalAlignment value) => obj.SetValue(HeaderHorizontalAlignmentProperty, value);

        public static VerticalAlignment GetHeaderVerticalAlignment(Control obj) => obj.GetValue(HeaderVerticalAlignmentProperty);
        public static void SetHeaderVerticalAlignment(Control obj, VerticalAlignment value) => obj.SetValue(HeaderVerticalAlignmentProperty, value);

        public static double GetMinWidth(Control obj) => obj.GetValue(MinWidthProperty);
        public static void SetMinWidth(Control obj, double value) => obj.SetValue(MinWidthProperty, value);

        public static double GetWidth(Control obj) => obj.GetValue(WidthProperty);
        public static void SetWidth(Control obj, double value) => obj.SetValue(WidthProperty, value);

        public static double GetMaxWidth(Control obj) => obj.GetValue(MaxWidthProperty);
        public static void SetMaxWidth(Control obj, double value) => obj.SetValue(MaxWidthProperty, value);
    }
}