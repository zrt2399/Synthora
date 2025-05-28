using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class DropShadowChrome : ContentControl
    {
        public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
          AvaloniaProperty.Register<DropShadowChrome, BoxShadows>(nameof(BoxShadow));
 
        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }
    }
}