using Avalonia;
using Avalonia.Controls;

namespace Synthora.Attaches
{
    public class ButtonAttach
    { 
        public static readonly AttachedProperty<bool> ShowShadowOnInteractionProperty =
            AvaloniaProperty.RegisterAttached<ButtonAttach, Control, bool>("ShowShadowOnInteraction", defaultValue: true);

        public static bool GetShowShadowOnInteraction(Control obj) => obj.GetValue(ShowShadowOnInteractionProperty);
        public static void SetShowShadowOnInteraction(Control obj, bool value) => obj.SetValue(ShowShadowOnInteractionProperty, value);
    }
}