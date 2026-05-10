using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Attaches
{
    public class ButtonAttach
    { 
        public static readonly AttachedProperty<bool> ShowShadowProperty =
            AvaloniaProperty.RegisterAttached<ButtonAttach, Control, bool>("ShowShadow", defaultValue: true);

        public static bool GetShowShadow(Control obj) => obj.GetValue(ShowShadowProperty);
        public static void SetShowShadow(Control obj, bool value) => obj.SetValue(ShowShadowProperty, value);
    }
}