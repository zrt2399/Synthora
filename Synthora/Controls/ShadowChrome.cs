using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Displays content with a configurable shadow.
    /// </summary>
    public class ShadowChrome : ContentControl
    {
        /// <summary>
        /// Defines the <see cref="BoxShadow"/> property.
        /// </summary>
        public static readonly StyledProperty<BoxShadows> BoxShadowProperty = 
            AvaloniaProperty.Register<ShadowChrome, BoxShadows>(nameof(BoxShadow));
         
        /// <summary>
        /// Defines the <see cref="ShowShadowOnInteraction"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowShadowOnInteractionProperty = 
            AvaloniaProperty.Register<ShadowChrome, bool>(nameof(ShowShadowOnInteraction));
        
        /// <summary>
        /// Defines the <see cref="InteractionBoxShadow"/> property.
        /// </summary>
        public static readonly StyledProperty<BoxShadows> InteractionBoxShadowProperty = 
            AvaloniaProperty.Register<ShadowChrome, BoxShadows>(nameof(InteractionBoxShadow));

        /// <summary>
        /// Gets or sets the shadow applied to the chrome.
        /// </summary>
        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the interaction shadow is shown during pointer or focus interaction.
        /// </summary>
        public bool ShowShadowOnInteraction
        {
            get => GetValue(ShowShadowOnInteractionProperty);
            set => SetValue(ShowShadowOnInteractionProperty, value);
        }

        /// <summary>
        /// Gets or sets the shadow applied during pointer or focus interaction.
        /// </summary>
        public BoxShadows InteractionBoxShadow
        {
            get => GetValue(InteractionBoxShadowProperty);
            set => SetValue(InteractionBoxShadowProperty, value);
        }
    }
}