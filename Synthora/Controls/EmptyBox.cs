using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents an empty state control with an icon and message.
    /// </summary>
    public class EmptyBox : TemplatedControl
    {
        /// <summary>
        /// Defines the <see cref="IconWidth"/> property.
        /// Determines the width of the empty state icon.
        /// </summary>
        public static readonly StyledProperty<double> IconWidthProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(IconWidth), 60d);

        /// <summary>
        /// Defines the <see cref="IconHeight"/> property.
        /// Determines the height of the empty state icon.
        /// </summary>
        public static readonly StyledProperty<double> IconHeightProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(IconHeight), 40d);

        /// <summary>
        /// Defines the <see cref="Text"/> property.
        /// Determines the text displayed below the empty state icon.
        /// </summary>
        public static readonly StyledProperty<string?> TextProperty =
            TextBlock.TextProperty.AddOwner<EmptyBox>();

        /// <summary>
        /// Defines the <see cref="Spacing"/> property.
        /// Determines the spacing between the empty state icon and text.
        /// </summary>
        public static readonly StyledProperty<double> SpacingProperty =
            StackPanel.SpacingProperty.AddOwner<EmptyBox>();

        /// <summary>
        /// Gets or sets the width of the empty state icon.
        /// </summary>
        public double IconWidth
        {
            get => GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the height of the empty state icon.
        /// </summary>
        public double IconHeight
        {
            get => GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between the empty state icon and text.
        /// </summary>
        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed below the empty state icon.
        /// </summary>
        public string? Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
