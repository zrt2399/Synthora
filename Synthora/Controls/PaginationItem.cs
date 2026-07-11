using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a generated page button container used by <see cref="Pagination"/>.
    /// </summary>
    [PseudoClasses(pcCurrent, pcJumpPrevious, pcJumpNext)]
    public class PaginationItem : Button
    {
        private const string pcCurrent = ":current";
        private const string pcJumpPrevious = ":jump-previous";
        private const string pcJumpNext = ":jump-next";

        /// <summary>
        /// Defines the <see cref="PageIndex"/> property.
        /// </summary>
        public static readonly StyledProperty<int> PageIndexProperty =
            AvaloniaProperty.Register<PaginationItem, int>(nameof(PageIndex), -1);

        /// <summary>
        /// Defines the <see cref="IsCurrent"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsCurrentProperty =
            AvaloniaProperty.Register<PaginationItem, bool>(nameof(IsCurrent));

        /// <summary>
        /// Defines the <see cref="IsJumpPrevious"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsJumpPreviousProperty =
            AvaloniaProperty.Register<PaginationItem, bool>(nameof(IsJumpPrevious));

        /// <summary>
        /// Defines the <see cref="IsJumpNext"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsJumpNextProperty =
            AvaloniaProperty.Register<PaginationItem, bool>(nameof(IsJumpNext));

        internal Pagination? Owner { get; set; }

        /// <summary>
        /// Gets the zero-based target page index.
        /// </summary>
        public int PageIndex
        {
            get => GetValue(PageIndexProperty);
            internal set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        /// Gets whether this item represents the current page.
        /// </summary>
        public bool IsCurrent
        {
            get => GetValue(IsCurrentProperty);
            internal set => SetValue(IsCurrentProperty, value);
        }

        /// <summary>
        /// Gets whether this item jumps to the previous page group.
        /// </summary>
        public bool IsJumpPrevious
        {
            get => GetValue(IsJumpPreviousProperty);
            internal set => SetValue(IsJumpPreviousProperty, value);
        }

        /// <summary>
        /// Gets whether this item jumps to the next page group.
        /// </summary>
        public bool IsJumpNext
        {
            get => GetValue(IsJumpNextProperty);
            internal set => SetValue(IsJumpNextProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == IsCurrentProperty ||
                change.Property == IsJumpPreviousProperty ||
                change.Property == IsJumpNextProperty)
            {
                UpdatePseudoClasses();
            }
        }

        protected override void OnClick()
        {
            base.OnClick();

            Owner?.SelectPage(PageIndex);
        }

        private void UpdatePseudoClasses()
        {
            PseudoClasses.Set(pcCurrent, IsCurrent);
            PseudoClasses.Set(pcJumpPrevious, IsJumpPrevious);
            PseudoClasses.Set(pcJumpNext, IsJumpNext);
        }
    }
}