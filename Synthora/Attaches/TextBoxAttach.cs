using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class TextBoxAttach
    {
        public static readonly AttachedProperty<object?> TitleProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, object?>("Title");

        public static void SetTitle(Control obj, object? value)
        {
            obj.SetValue(TitleProperty, value);
        }

        public static object? GetTitle(Control obj)
        {
            return obj.GetValue(TitleProperty);
        }
        
        public static readonly AttachedProperty<Dock> TitlePlacementProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, Dock>("TitlePlacement", Dock.Top);

        public static void SetTitlePlacement(Control obj, Dock value)
        {
            obj.SetValue(TitlePlacementProperty, value);
        }

        public static Dock GetTitlePlacement(Control obj)
        {
            return obj.GetValue(TitlePlacementProperty);
        }        
 
        public static readonly AttachedProperty<HorizontalAlignment> TitleHorizontalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, HorizontalAlignment>("TitleHorizontalAlignment", HorizontalAlignment.Left);

        public static void SetTitleHorizontalAlignment(Control obj, HorizontalAlignment value)
        {
            obj.SetValue(TitleHorizontalAlignmentProperty, value);
        }

        public static HorizontalAlignment GetTitleHorizontalAlignment(Control obj)
        {
            return obj.GetValue(TitleHorizontalAlignmentProperty);
        }   
        
        public static readonly AttachedProperty<VerticalAlignment> TitleVerticalAlignmentProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, Control, VerticalAlignment>("TitleVerticalAlignment", VerticalAlignment.Center);

        public static void SetTitleVerticalAlignment(Control obj, VerticalAlignment value)
        {
            obj.SetValue(TitleVerticalAlignmentProperty, value);
        }

        public static VerticalAlignment GetTitleVerticalAlignment(Control obj)
        {
            return obj.GetValue(TitleVerticalAlignmentProperty);
        }
    }
}