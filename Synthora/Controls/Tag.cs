using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

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
    [PseudoClasses(pcNone, pcInformation, pcQuestion, pcSuccess, pcWarning, pcDanger, pcError, pcSolid, pcCircular)]
    public class Tag : ContentControl
    {
        private const string pcNone = ":none";
        private const string pcInformation = ":information";
        private const string pcQuestion = ":question";
        private const string pcSuccess = ":success";
        private const string pcWarning = ":warning";
        private const string pcDanger = ":danger";
        private const string pcError = ":error";
        private const string pcSolid = ":solid";
        private const string pcCircular = ":circular";

        /// <summary>
        /// Defines the <see cref="TagType"/> property.
        /// </summary>
        public static readonly StyledProperty<TagType> TagTypeProperty =
            AvaloniaProperty.Register<Tag, TagType>(nameof(TagType));

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

        public Tag()
        {
            UpdatePseudoClasses();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == TagTypeProperty ||
                change.Property == IsSolidProperty ||
                change.Property == IsCircularProperty)
            {
                UpdatePseudoClasses();
            }
        }

        private void UpdatePseudoClasses()
        {
            var tagType = TagType;
            PseudoClasses.Set(pcNone, tagType == TagType.None);
            PseudoClasses.Set(pcInformation, tagType == TagType.Information);
            PseudoClasses.Set(pcQuestion, tagType == TagType.Question);
            PseudoClasses.Set(pcSuccess, tagType == TagType.Success);
            PseudoClasses.Set(pcWarning, tagType == TagType.Warning);
            PseudoClasses.Set(pcDanger, tagType == TagType.Danger);
            PseudoClasses.Set(pcError, tagType == TagType.Error);

            PseudoClasses.Set(pcSolid, IsSolid);
            PseudoClasses.Set(pcCircular, IsCircular);
        }
    }
}