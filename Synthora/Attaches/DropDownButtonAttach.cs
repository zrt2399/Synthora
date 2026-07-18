using Avalonia;
using Avalonia.Controls;

namespace Synthora.Attaches
{
    public class DropDownButtonAttach
    {
        public static readonly AttachedProperty<object?> DropDownIconProperty =
            AvaloniaProperty.RegisterAttached<DropDownButtonAttach, Control, object?>("DropDownIcon");

        public static readonly AttachedProperty<bool> DropDownIconAnimationEnabledProperty =
            AvaloniaProperty.RegisterAttached<DropDownButtonAttach, Control, bool>("DropDownIconAnimationEnabled", defaultValue: true);

        public static object? GetDropDownIcon(Control obj) => obj.GetValue(DropDownIconProperty);
        public static void SetDropDownIcon(Control obj, object? value) => obj.SetValue(DropDownIconProperty, value);

        public static bool GetDropDownIconAnimationEnabled(Control obj) => obj.GetValue(DropDownIconAnimationEnabledProperty);
        public static void SetDropDownIconAnimationEnabled(Control obj, bool value) => obj.SetValue(DropDownIconAnimationEnabledProperty, value);
    }
}