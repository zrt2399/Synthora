using Avalonia;
using Avalonia.Controls;

namespace Synthora.Attaches
{
    public class DropDownButtonAttach
    {
        public static readonly AttachedProperty<object?> DropDownIconProperty =
            AvaloniaProperty.RegisterAttached<DropDownButtonAttach, Control, object?>("DropDownIcon");

        public static readonly AttachedProperty<bool> EnableDropDownIconAnimationProperty =
            AvaloniaProperty.RegisterAttached<DropDownButtonAttach, Control, bool>("EnableDropDownIconAnimation", defaultValue: true);

        public static object? GetDropDownIcon(Control obj) => obj.GetValue(DropDownIconProperty);
        public static void SetDropDownIcon(Control obj, object? value) => obj.SetValue(DropDownIconProperty, value);

        public static bool GetEnableDropDownIconAnimation(Control obj) => obj.GetValue(EnableDropDownIconAnimationProperty);
        public static void SetEnableDropDownIconAnimation(Control obj, bool value) => obj.SetValue(EnableDropDownIconAnimationProperty, value);
    }
}