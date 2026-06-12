using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    /// <summary>
    /// Defines the visual type used by <see cref="Badge"/>.
    /// </summary>
    public enum BadgeType
    {
        None,
        Information,
        Question,
        Success,
        Warning,
        Danger,
        Error
    }

    /// <summary>
    /// Represents a compact status label.
    /// </summary>
    [PseudoClasses(pcNone, pcInformation, pcQuestion, pcSuccess, pcWarning, pcDanger, pcError)]
    public class Badge : ContentControl
    {
        private const string pcNone = ":none";
        private const string pcInformation = ":information";
        private const string pcQuestion = ":question";
        private const string pcSuccess = ":success";
        private const string pcWarning = ":warning";
        private const string pcDanger = ":danger";
        private const string pcError = ":error";

        /// <summary>
        /// Defines the <see cref="BadgeType"/> property.
        /// </summary>
        public static readonly StyledProperty<BadgeType> BadgeTypeProperty =
            AvaloniaProperty.Register<Badge, BadgeType>(nameof(BadgeType), BadgeType.Information);

        /// <summary>
        /// Defines the <see cref="IsSolid"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsSolidProperty =
            AvaloniaProperty.Register<Badge, bool>(nameof(IsSolid));

        /// <summary>
        /// Defines the <see cref="IsCircular"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsCircularProperty =
            AvaloniaProperty.Register<Badge, bool>(nameof(IsCircular));

        static Badge()
        {
            BadgeTypeProperty.Changed.AddClassHandler<Badge, BadgeType>((s, e) => s.UpdatePseudoClasses());
        }

        /// <summary>
        /// Gets or sets the visual type of the badge.
        /// </summary>
        public BadgeType BadgeType
        {
            get => GetValue(BadgeTypeProperty);
            set => SetValue(BadgeTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the badge uses a solid filled background.
        /// </summary>
        public bool IsSolid
        {
            get => GetValue(IsSolidProperty);
            set => SetValue(IsSolidProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the badge uses a circular corner radius.
        /// </summary>
        public bool IsCircular
        {
            get => GetValue(IsCircularProperty);
            set => SetValue(IsCircularProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            UpdatePseudoClasses();
        }

        private void UpdatePseudoClasses()
        {
            PseudoClasses.Set(pcNone, BadgeType == BadgeType.None);
            PseudoClasses.Set(pcInformation, BadgeType == BadgeType.Information);
            PseudoClasses.Set(pcQuestion, BadgeType == BadgeType.Question);
            PseudoClasses.Set(pcSuccess, BadgeType == BadgeType.Success);
            PseudoClasses.Set(pcWarning, BadgeType == BadgeType.Warning);
            PseudoClasses.Set(pcDanger, BadgeType == BadgeType.Danger);
            PseudoClasses.Set(pcError, BadgeType == BadgeType.Error);
        }
    }
}