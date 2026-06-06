using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class ShadowChrome : ContentControl
    {
        public static readonly StyledProperty<BoxShadows> BoxShadowProperty = 
            AvaloniaProperty.Register<ShadowChrome, BoxShadows>(nameof(BoxShadow));
 
        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }
    }
}