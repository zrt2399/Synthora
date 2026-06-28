using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a compact status indicator.
    /// </summary>
    [PseudoClasses(pcNone, pcInformation, pcQuestion, pcSuccess, pcWarning, pcError)]
    public class StatusIndicator : IconBase
    {
        private const string pcNone = ":none";
        private const string pcInformation = ":information";
        private const string pcQuestion = ":question";
        private const string pcSuccess = ":success";
        private const string pcWarning = ":warning";
        private const string pcError = ":error";

        /// <summary>
        /// Defines the <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty =
            AvaloniaProperty.Register<StatusIndicator, double>(nameof(StrokeThickness));

        static StatusIndicator()
        { 
            IconTypeProperty.Changed.AddClassHandler<StatusIndicator, IconType>((s, e) => s.UpdatePseudoClasses());
        }

        /// <summary>
        /// Gets or sets the thickness of the status border.
        /// </summary>
        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <inheritdoc/>
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