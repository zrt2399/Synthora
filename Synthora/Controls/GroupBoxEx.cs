using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a control that displays content with a header, providing extended styling and shadow capabilities.
    /// </summary>
    public class GroupBoxEx : HeaderedContentControl
    {
        /// <summary>
        /// Defines the <see cref="HeaderPadding"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> HeaderPaddingProperty =
            AvaloniaProperty.Register<GroupBoxEx, Thickness>(nameof(HeaderPadding));

        /// <summary>
        /// Defines the <see cref="HeaderBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
            AvaloniaProperty.Register<GroupBoxEx, IBrush?>(nameof(HeaderBackground));

        /// <summary>
        /// Defines the <see cref="BoxShadow"/> property.
        /// </summary>
        public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
            AvaloniaProperty.Register<GroupBoxEx, BoxShadows>(nameof(BoxShadow));

        /// <summary>
        /// Defines the <see cref="HeaderDividerThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<double> HeaderDividerThicknessProperty =
            AvaloniaProperty.Register<GroupBoxEx, double>(nameof(HeaderDividerThickness));

        /// <summary>
        /// Defines the <see cref="HeaderPlacement"/> property.
        /// </summary>
        public static readonly StyledProperty<Dock> HeaderPlacementProperty =
            AvaloniaProperty.Register<GroupBoxEx, Dock>(nameof(HeaderPlacement));

        /// <summary>
        /// Gets or sets the padding for the header content.
        /// </summary>
        public Thickness HeaderPadding
        {
            get => GetValue(HeaderPaddingProperty);
            set => SetValue(HeaderPaddingProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush for the header.
        /// </summary>
        public IBrush? HeaderBackground
        {
            get => GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the box shadow applied to the control.
        /// </summary>
        public BoxShadows BoxShadow
        {
            get => GetValue(BoxShadowProperty);
            set => SetValue(BoxShadowProperty, value);
        }

        /// <summary>
        /// Gets or sets the thickness of the divider below the header.
        /// </summary>
        public double HeaderDividerThickness
        {
            get => GetValue(HeaderDividerThicknessProperty);
            set => SetValue(HeaderDividerThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the placement of the header relative to the content.
        /// </summary>
        public Dock HeaderPlacement
        {
            get => GetValue(HeaderPlacementProperty);
            set => SetValue(HeaderPlacementProperty, value);
        }
    }
}