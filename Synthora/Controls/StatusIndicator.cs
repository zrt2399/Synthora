using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a compact status indicator.
    /// </summary>
    [PseudoClasses(pcNone, pcInformation, pcQuestion, pcSuccess, pcWarning, pcError)]
    public class StatusIndicator : ContentControl
    {
        private const string pcNone = ":none";
        private const string pcInformation = ":information";
        private const string pcQuestion = ":question";
        private const string pcSuccess = ":success";
        private const string pcWarning = ":warning";
        private const string pcError = ":error";

        /// <summary>
        /// Defines the <see cref="IconType"/> property.
        /// </summary>
        public static readonly StyledProperty<IconType> IconTypeProperty =
            AvaloniaProperty.Register<StatusIndicator, IconType>(nameof(IconType));

        /// <summary>
        /// Defines the <see cref="IconPlacement"/> property.
        /// </summary>
        public static readonly StyledProperty<Dock> IconPlacementProperty =
            AvaloniaProperty.Register<StatusIndicator, Dock>(nameof(IconPlacement));

        /// <summary>
        /// Defines the <see cref="IconWidth"/> property.
        /// </summary>
        public static readonly StyledProperty<double> IconWidthProperty =
            AvaloniaProperty.Register<StatusIndicator, double>(nameof(IconWidth), 20d);

        /// <summary>
        /// Defines the <see cref="IconHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> IconHeightProperty =
            AvaloniaProperty.Register<StatusIndicator, double>(nameof(IconHeight), 20d);

        /// <summary>
        /// Defines the <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty =
            AvaloniaProperty.Register<StatusIndicator, double>(nameof(StrokeThickness));

        /// <summary>
        /// Defines the <see cref="HighlightMargin"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> HighlightMarginProperty =
            AvaloniaProperty.Register<StatusIndicator, Thickness>(nameof(HighlightMargin));

        /// <summary>
        /// Defines the <see cref="TextWrapping"/> property.
        /// </summary>
        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            AvaloniaProperty.Register<StatusIndicator, TextWrapping>(nameof(TextWrapping));

        static StatusIndicator()
        {
            IconTypeProperty.Changed.AddClassHandler<StatusIndicator, IconType>((s, e) => s.UpdatePseudoClasses());
        }

        /// <summary>
        /// Gets or sets the status type.
        /// </summary>
        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets the placement of the status icon.
        /// </summary>
        public Dock IconPlacement
        {
            get => GetValue(IconPlacementProperty);
            set => SetValue(IconPlacementProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the status icon.
        /// </summary>
        public double IconWidth
        {
            get => GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the height of the status icon.
        /// </summary>
        public double IconHeight
        {
            get => GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the thickness of the status border.
        /// </summary>
        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the margin of the status highlight.
        /// </summary>
        public Thickness HighlightMargin
        {
            get => GetValue(HighlightMarginProperty);
            set => SetValue(HighlightMarginProperty, value);
        }

        /// <summary>
        /// Gets or sets how the content text wraps.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get => GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            UpdatePseudoClasses();
        }

        private void UpdatePseudoClasses()
        {
            PseudoClasses.Set(pcNone, IconType == IconType.None);
            PseudoClasses.Set(pcInformation, IconType == IconType.Information);
            PseudoClasses.Set(pcQuestion, IconType == IconType.Question);
            PseudoClasses.Set(pcSuccess, IconType == IconType.Success);
            PseudoClasses.Set(pcWarning, IconType == IconType.Warning);
            PseudoClasses.Set(pcError, IconType == IconType.Error);
        }
    }
}