using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Attaches
{
    public class BoxShadowAttach
    {
        public static readonly AttachedProperty<BoxShadows> BoxShadowProperty = 
            AvaloniaProperty.RegisterAttached<BoxShadowAttach, Control, BoxShadows>("BoxShadow");

        public static BoxShadows GetBoxShadow(Control obj) => obj.GetValue(BoxShadowProperty);
        public static void SetBoxShadow(Control obj, BoxShadows value) => obj.SetValue(BoxShadowProperty, value);
    }
}