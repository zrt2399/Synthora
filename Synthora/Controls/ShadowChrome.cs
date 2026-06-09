using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class ShadowChrome : ContentControl
    {
        public static readonly StyledProperty<BoxShadows> BoxShadowProperty = 
            AvaloniaProperty.Register<ShadowChrome, BoxShadows>(nameof(BoxShadow));
         
        public static readonly StyledProperty<bool> ShowShadowOnInteractionProperty = 
            AvaloniaProperty.Register<ShadowChrome, bool>(nameof(ShowShadowOnInteraction));
        
        public static readonly StyledProperty<BoxShadows> InteractionBoxShadowProperty = 
            AvaloniaProperty.Register<ShadowChrome, BoxShadows>(nameof(InteractionBoxShadow));

        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }

        public bool ShowShadowOnInteraction
        {
            get => GetValue(ShowShadowOnInteractionProperty);
            set => SetValue(ShowShadowOnInteractionProperty, value);
        }

        public BoxShadows InteractionBoxShadow
        {
            get => GetValue(InteractionBoxShadowProperty);
            set => SetValue(InteractionBoxShadowProperty, value);
        }
    }
}