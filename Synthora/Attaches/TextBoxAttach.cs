using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class TextBoxAttach
    {
        public static readonly AttachedProperty<object?> TitleProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, object?>("Title");

        public static readonly AttachedProperty<Dock> TitlePlacementProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, Dock>("TitlePlacement", Dock.Top);

        public static readonly AttachedProperty<HorizontalAlignment> TitleHorizontalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, HorizontalAlignment>("TitleHorizontalAlignment", HorizontalAlignment.Left);

        public static readonly AttachedProperty<VerticalAlignment> TitleVerticalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, VerticalAlignment>("TitleVerticalAlignment", VerticalAlignment.Center);

        public static readonly AttachedProperty<double> MinWidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("MinWidth");

        public static readonly AttachedProperty<double> WidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("Width", double.NaN);

        public static readonly AttachedProperty<double> MaxWidthProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, double>("MaxWidth", double.PositiveInfinity);

        public static object? GetTitle(Control obj) => obj.GetValue(TitleProperty);
        public static void SetTitle(Control obj, object? value) => obj.SetValue(TitleProperty, value);

        public static Dock GetTitlePlacement(Control obj) => obj.GetValue(TitlePlacementProperty);
        public static void SetTitlePlacement(Control obj, Dock value) => obj.SetValue(TitlePlacementProperty, value);

        public static HorizontalAlignment GetTitleHorizontalAlignment(Control obj) => obj.GetValue(TitleHorizontalAlignmentProperty);
        public static void SetTitleHorizontalAlignment(Control obj, HorizontalAlignment value) => obj.SetValue(TitleHorizontalAlignmentProperty, value);

        public static VerticalAlignment GetTitleVerticalAlignment(Control obj) => obj.GetValue(TitleVerticalAlignmentProperty);
        public static void SetTitleVerticalAlignment(Control obj, VerticalAlignment value) => obj.SetValue(TitleVerticalAlignmentProperty, value);

        public static double GetMinWidth(Control obj) => obj.GetValue(MinWidthProperty);
        public static void SetMinWidth(Control obj, double value) => obj.SetValue(MinWidthProperty, value);

        public static double GetWidth(Control obj) => obj.GetValue(WidthProperty);
        public static void SetWidth(Control obj, double value) => obj.SetValue(WidthProperty, value);

        public static double GetMaxWidth(Control obj) => obj.GetValue(MaxWidthProperty);
        public static void SetMaxWidth(Control obj, double value) => obj.SetValue(MaxWidthProperty, value); 
    }
}