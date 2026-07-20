using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a banner control for displaying messages with different severity levels.
    /// </summary>
    [PseudoClasses(pcInformation, pcQuestion, pcSuccess, pcWarning, pcError, pcNone)]
    public class Banner : HeaderedContentControl
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
            AvaloniaProperty.Register<Banner, IconType>(nameof(IconType));

        /// <summary>
        /// Defines the <see cref="ShowCloseButton"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowCloseButtonProperty =
            AvaloniaProperty.Register<Banner, bool>(nameof(ShowCloseButton), true);

        /// <summary>
        /// Defines the <see cref="ShowIcon"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowIconProperty =
            AvaloniaProperty.Register<Banner, bool>(nameof(ShowIcon), true);

        /// <summary>
        /// Defines the <see cref="ShowAccentBar"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowAccentBarProperty =
            AvaloniaProperty.Register<Banner, bool>(nameof(ShowAccentBar));

        /// <summary>
        /// Defines the <see cref="TextWrapping"/> property.
        /// </summary>
        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            AvaloniaProperty.Register<Banner, TextWrapping>(nameof(TextWrapping), TextWrapping.Wrap);

        static Banner()
        {
            IconTypeProperty.Changed.AddClassHandler<Banner, IconType>((s, e) => s.UpdatePseudoClasses());
        }

        /// <summary>
        /// Gets or sets the icon type of the banner.
        /// </summary>
        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to show the close button.
        /// </summary>
        public bool ShowCloseButton
        {
            get => GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to show the icon.
        /// </summary>
        public bool ShowIcon
        {
            get => GetValue(ShowIconProperty);
            set => SetValue(ShowIconProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to show the accent bar.
        /// </summary>
        public bool ShowAccentBar
        {
            get => GetValue(ShowAccentBarProperty);
            set => SetValue(ShowAccentBarProperty, value);
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

        public void Show()
        {
            SetCurrentValue(IsVisibleProperty, true);
        }

        public void Close()
        {
            SetCurrentValue(IsVisibleProperty, false);
        }
    }
}