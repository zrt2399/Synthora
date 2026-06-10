using Avalonia;
using Avalonia.Controls;

namespace Synthora.Attaches
{
    public class DateTimePickerAttach
    {
        public static readonly AttachedProperty<bool> IsDropDownOpenProperty =
            AvaloniaProperty.RegisterAttached<DateTimePickerAttach, Control, bool>("IsDropDownOpen");

        public static bool GetIsDropDownOpen(Control obj) => obj.GetValue(IsDropDownOpenProperty);
        public static void SetIsDropDownOpen(Control obj, bool value) => obj.SetValue(IsDropDownOpenProperty, value);
    }
}