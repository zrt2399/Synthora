using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    /// <summary>
    /// Defines the visual type used by <see cref="Tag"/>.
    /// </summary>
    public enum TagType
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
    public class Tag : ContentControl
    {
        private const string pcNone = ":none";
        private const string pcInformation = ":information";
        private const string pcQuestion = ":question";
        private const string pcSuccess = ":success";
        private const string pcWarning = ":warning";
        private const string pcDanger = ":danger";
        private const string pcError = ":error";

        /// <summary>
        /// Defines the <see cref="TagType"/> property.
        /// </summary>
        public static readonly StyledProperty<TagType> TagTypeProperty =
            AvaloniaProperty.Register<Tag, TagType>(nameof(TagType), TagType.Information);

        /// <summary>
        /// Defines the <see cref="IsSolid"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsSolidProperty =
            AvaloniaProperty.Register<Tag, bool>(nameof(IsSolid));

        /// <summary>
        /// Defines the <see cref="IsCircular"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsCircularProperty =
            AvaloniaProperty.Register<Tag, bool>(nameof(IsCircular));

        static Tag()
        {
            TagTypeProperty.Changed.AddClassHandler<Tag, TagType>((s, e) => s.UpdatePseudoClasses());
        }

        /// <summary>
        /// Gets or sets the visual type of the tag.
        /// </summary>
        public TagType TagType
        {
            get => GetValue(TagTypeProperty);
            set => SetValue(TagTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the tag uses a solid filled background.
        /// </summary>
        public bool IsSolid
        {
            get => GetValue(IsSolidProperty);
            set => SetValue(IsSolidProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the tag uses a circular corner radius.
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
            PseudoClasses.Set(pcNone, TagType == TagType.None);
            PseudoClasses.Set(pcInformation, TagType == TagType.Information);
            PseudoClasses.Set(pcQuestion, TagType == TagType.Question);
            PseudoClasses.Set(pcSuccess, TagType == TagType.Success);
            PseudoClasses.Set(pcWarning, TagType == TagType.Warning);
            PseudoClasses.Set(pcDanger, TagType == TagType.Danger);
            PseudoClasses.Set(pcError, TagType == TagType.Error);
        }
    }
}