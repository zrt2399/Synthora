using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;

namespace Synthora.Controls
{
    /// <summary>
    /// Provides navigation across a paged collection of items.
    /// </summary>
    [PseudoClasses(pcFirstPage, pcLastPage, pcEmpty)]
    public class Pagination : ItemsControl
    {
        private const string pcFirstPage = ":first-page";
        private const string pcLastPage = ":last-page";
        private const string pcEmpty = ":empty";
        private const int JumpPreviousPageIndex = -1;
        private const int JumpNextPageIndex = -2;
        private static readonly IReadOnlyList<int> DefaultPageSizeOptions = [10, 20, 50, 100, 200, 500, 1000];

        private TextBox? _quickJumpBox;
        private int _pageCount;
        private int _startItemIndex;
        private int _endItemIndex;
        private bool _canGoPrevious;
        private bool _canGoNext;
        private bool _canConfirmQuickJump;
        private string _statusText = string.Empty;
        /// <summary>
        /// Defines the <see cref="PageIndex"/> property.
        /// </summary>
        public static readonly StyledProperty<int> PageIndexProperty =
            AvaloniaProperty.Register<Pagination, int>(nameof(PageIndex), defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Defines the <see cref="PageSize"/> property.
        /// </summary>
        public static readonly StyledProperty<int> PageSizeProperty =
            AvaloniaProperty.Register<Pagination, int>(nameof(PageSize), defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Defines the <see cref="TotalItemCount"/> property.
        /// </summary>
        public static readonly StyledProperty<int> TotalItemCountProperty =
            AvaloniaProperty.Register<Pagination, int>(nameof(TotalItemCount), defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Defines the <see cref="DisplayPageCount"/> property.
        /// </summary>
        public static readonly StyledProperty<int> DisplayPageCountProperty =
            AvaloniaProperty.Register<Pagination, int>(
                nameof(DisplayPageCount),
                10,
                coerce: (_, value) => Math.Max(5, value));

        /// <summary>
        /// Defines the <see cref="PageSizeOptions"/> property.
        /// </summary>
        public static readonly StyledProperty<IEnumerable<int>?> PageSizeOptionsProperty =
            AvaloniaProperty.Register<Pagination, IEnumerable<int>?>(nameof(PageSizeOptions));

        /// <summary>
        /// Defines the <see cref="ShowTotalItemCount"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowTotalItemCountProperty =
            AvaloniaProperty.Register<Pagination, bool>(nameof(ShowTotalItemCount));

        /// <summary>
        /// Defines the <see cref="ShowPageSizeSelector"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowPageSizeSelectorProperty =
            AvaloniaProperty.Register<Pagination, bool>(nameof(ShowPageSizeSelector));

        /// <summary>
        /// Defines the <see cref="ShowQuickJumper"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowQuickJumperProperty =
            AvaloniaProperty.Register<Pagination, bool>(nameof(ShowQuickJumper));

        /// <summary>
        /// Defines the <see cref="ShowFirstLastButtons"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowFirstLastButtonsProperty =
            AvaloniaProperty.Register<Pagination, bool>(nameof(ShowFirstLastButtons));

        /// <summary>
        /// Defines the <see cref="StatusTextFormat"/> property.
        /// </summary>
        public static readonly StyledProperty<string> StatusTextFormatProperty =
            AvaloniaProperty.Register<Pagination, string>(nameof(StatusTextFormat));

        /// <summary>
        /// Defines the <see cref="TotalTextFormat"/> property.
        /// </summary>
        public static readonly StyledProperty<string> TotalTextFormatProperty =
            AvaloniaProperty.Register<Pagination, string>(nameof(TotalTextFormat));

        /// <summary>
        /// Defines the <see cref="PageSizeUnitText"/> property.
        /// </summary>
        public static readonly StyledProperty<string> PageSizeUnitTextProperty =
            AvaloniaProperty.Register<Pagination, string>(nameof(PageSizeUnitText));

        /// <summary>
        /// Defines the <see cref="QuickJumpButtonText"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> QuickJumpButtonTextProperty =
            AvaloniaProperty.Register<Pagination, string?>(nameof(QuickJumpButtonText));

        /// <summary>
        /// Defines the <see cref="PageCount"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, int> PageCountProperty =
            AvaloniaProperty.RegisterDirect<Pagination, int>(nameof(PageCount), o => o.PageCount);

        /// <summary>
        /// Defines the <see cref="StartItemIndex"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, int> StartItemIndexProperty =
            AvaloniaProperty.RegisterDirect<Pagination, int>(nameof(StartItemIndex), o => o.StartItemIndex);

        /// <summary>
        /// Defines the <see cref="EndItemIndex"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, int> EndItemIndexProperty =
            AvaloniaProperty.RegisterDirect<Pagination, int>(nameof(EndItemIndex), o => o.EndItemIndex);

        /// <summary>
        /// Defines the <see cref="StatusText"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, string> StatusTextProperty =
            AvaloniaProperty.RegisterDirect<Pagination, string>(nameof(StatusText), o => o.StatusText);

        /// <summary>
        /// Defines the <see cref="CanGoPrevious"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, bool> CanGoPreviousProperty =
            AvaloniaProperty.RegisterDirect<Pagination, bool>(nameof(CanGoPrevious), o => o.CanGoPrevious);

        /// <summary>
        /// Defines the <see cref="CanGoNext"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, bool> CanGoNextProperty =
            AvaloniaProperty.RegisterDirect<Pagination, bool>(nameof(CanGoNext), o => o.CanGoNext);

        /// <summary>
        /// Defines the <see cref="CanConfirmQuickJump"/> property.
        /// </summary>
        public static readonly DirectProperty<Pagination, bool> CanConfirmQuickJumpProperty =
            AvaloniaProperty.RegisterDirect<Pagination, bool>(nameof(CanConfirmQuickJump), o => o.CanConfirmQuickJump);

        /// <summary>
        /// Gets or sets the zero-based current page index.
        /// </summary>
        public int PageIndex
        {
            get => GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize
        {
            get => GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItemCount
        {
            get => GetValue(TotalItemCountProperty);
            set => SetValue(TotalItemCountProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum number of page buttons displayed in the page list, including ellipsis items.
        /// </summary>
        public int DisplayPageCount
        {
            get => GetValue(DisplayPageCountProperty);
            set => SetValue(DisplayPageCountProperty, value);
        }

        /// <summary>
        /// Gets or sets the page size values shown in the selector.
        /// </summary>
        public IEnumerable<int>? PageSizeOptions
        {
            get => GetValue(PageSizeOptionsProperty);
            set => SetValue(PageSizeOptionsProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the total item count text is shown.
        /// </summary>
        public bool ShowTotalItemCount
        {
            get => GetValue(ShowTotalItemCountProperty);
            set => SetValue(ShowTotalItemCountProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the page size selector is shown.
        /// </summary>
        public bool ShowPageSizeSelector
        {
            get => GetValue(ShowPageSizeSelectorProperty);
            set => SetValue(ShowPageSizeSelectorProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the quick jump input is shown.
        /// </summary>
        public bool ShowQuickJumper
        {
            get => GetValue(ShowQuickJumperProperty);
            set => SetValue(ShowQuickJumperProperty, value);
        }

        /// <summary>
        /// Gets or sets whether first and last page buttons are shown.
        /// </summary>
        public bool ShowFirstLastButtons
        {
            get => GetValue(ShowFirstLastButtonsProperty);
            set => SetValue(ShowFirstLastButtonsProperty, value);
        }

        /// <summary>
        /// Gets or sets the current range text format. Arguments are start item, end item, and total item count.
        /// </summary>
        public string StatusTextFormat
        {
            get => GetValue(StatusTextFormatProperty);
            set => SetValue(StatusTextFormatProperty, value);
        }

        /// <summary>
        /// Gets or sets the total item text format. The first argument is the total item count.
        /// </summary>
        public string TotalTextFormat
        {
            get => GetValue(TotalTextFormatProperty);
            set => SetValue(TotalTextFormatProperty, value);
        }

        /// <summary>
        /// Gets or sets the unit text shown after each page size option.
        /// </summary>
        public string PageSizeUnitText
        {
            get => GetValue(PageSizeUnitTextProperty);
            set => SetValue(PageSizeUnitTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the quick jump confirmation button text.
        /// </summary>
        public string? QuickJumpButtonText
        {
            get => GetValue(QuickJumpButtonTextProperty);
            set => SetValue(QuickJumpButtonTextProperty, value);
        }

        /// <summary>
        /// Gets the calculated page count.
        /// </summary>
        public int PageCount
        {
            get => _pageCount;
            private set => SetAndRaise(PageCountProperty, ref _pageCount, value);
        }

        /// <summary>
        /// Gets the one-based index of the first item displayed by the current page.
        /// </summary>
        public int StartItemIndex
        {
            get => _startItemIndex;
            private set => SetAndRaise(StartItemIndexProperty, ref _startItemIndex, value);
        }

        /// <summary>
        /// Gets the one-based index of the last item displayed by the current page.
        /// </summary>
        public int EndItemIndex
        {
            get => _endItemIndex;
            private set => SetAndRaise(EndItemIndexProperty, ref _endItemIndex, value);
        }

        /// <summary>
        /// Gets the formatted current range text.
        /// </summary>
        public string StatusText
        {
            get => _statusText;
            private set => SetAndRaise(StatusTextProperty, ref _statusText, value);
        }

        /// <summary>
        /// Gets whether the current page can move backward.
        /// </summary>
        public bool CanGoPrevious
        {
            get => _canGoPrevious;
            private set => SetAndRaise(CanGoPreviousProperty, ref _canGoPrevious, value);
        }

        /// <summary>
        /// Gets whether the current page can move forward.
        /// </summary>
        public bool CanGoNext
        {
            get => _canGoNext;
            private set => SetAndRaise(CanGoNextProperty, ref _canGoNext, value);
        }

        /// <summary>
        /// Gets whether the quick jump input can be confirmed.
        /// </summary>
        public bool CanConfirmQuickJump
        {
            get => _canConfirmQuickJump;
            private set => SetAndRaise(CanConfirmQuickJumpProperty, ref _canConfirmQuickJump, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagination"/> class.
        /// </summary>
        public Pagination()
        {
            SetCurrentValue(PageSizeOptionsProperty, DefaultPageSizeOptions);
            Refresh();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (_quickJumpBox != null)
            {
                _quickJumpBox.KeyDown -= OnQuickJumpBoxKeyDown;
            }

            base.OnApplyTemplate(e);

            _quickJumpBox = e.NameScope.Find<TextBox>("PART_QuickJumpBox");
            if (_quickJumpBox != null)
            {
                _quickJumpBox.KeyDown += OnQuickJumpBoxKeyDown;
            }
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == PageIndexProperty ||
                change.Property == PageSizeProperty ||
                change.Property == TotalItemCountProperty ||
                change.Property == DisplayPageCountProperty ||
                change.Property == StatusTextFormatProperty ||
                change.Property == TotalTextFormatProperty)
            {
                Refresh();
            }
        }

        protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        {
            return new PaginationItem();
        }

        protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
        {
            return NeedsContainer<PaginationItem>(item, out recycleKey);
        }

        protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
        {
            base.PrepareContainerForItemOverride(container, item, index);

            if (container is PaginationItem paginationItem && item is int pageIndex)
            {
                var isPage = pageIndex >= 0;
                var isJumpPrevious = pageIndex == JumpPreviousPageIndex;
                var isJumpNext = pageIndex == JumpNextPageIndex;
                paginationItem.Owner = this;
                paginationItem.SetCurrentValue(PaginationItem.PageIndexProperty, isPage ? pageIndex : GetJumpPageIndex(pageIndex));
                paginationItem.SetCurrentValue(PaginationItem.ContentProperty, isPage ? FormatPageNumber(pageIndex) : "...");
                paginationItem.SetCurrentValue(PaginationItem.IsCurrentProperty, pageIndex == PageIndex);
                paginationItem.SetCurrentValue(PaginationItem.IsJumpPreviousProperty, isJumpPrevious);
                paginationItem.SetCurrentValue(PaginationItem.IsJumpNextProperty, isJumpNext);
                paginationItem.SetCurrentValue(PaginationItem.IsEnabledProperty, isPage || isJumpPrevious || isJumpNext);
            }
        }

        protected override void ClearContainerForItemOverride(Control container)
        {
            if (container is PaginationItem paginationItem)
            {
                paginationItem.Owner = null;
                paginationItem.ClearValue(PaginationItem.PageIndexProperty);
                paginationItem.ClearValue(PaginationItem.ContentProperty);
                paginationItem.ClearValue(PaginationItem.IsCurrentProperty);
                paginationItem.ClearValue(PaginationItem.IsJumpPreviousProperty);
                paginationItem.ClearValue(PaginationItem.IsJumpNextProperty);
                paginationItem.ClearValue(PaginationItem.IsEnabledProperty);
            }

            base.ClearContainerForItemOverride(container);
        }

        /// <summary>
        /// Moves to the first page.
        /// </summary>
        public void FirstPage() => SetCurrentPage(0);

        /// <summary>
        /// Moves to the previous page.
        /// </summary>
        public void PreviousPage() => SetCurrentPage(PageIndex - 1);

        /// <summary>
        /// Moves to the next page.
        /// </summary>
        public void NextPage() => SetCurrentPage(PageIndex + 1);

        /// <summary>
        /// Moves to the last page.
        /// </summary>
        public void LastPage() => SetCurrentPage(PageCount - 1);

        private void Refresh()
        {
            var oldPageCount = PageCount;
            var pageCount = CalculatePageCount(TotalItemCount, PageSize);
            PageCount = pageCount;

            var normalizedPageIndex = NormalizePageIndex(PageIndex, pageCount, oldPageCount);
            if (normalizedPageIndex != PageIndex)
            {
                SetCurrentValue(PageIndexProperty, normalizedPageIndex);
                return;
            }

            UpdateCurrentRange();
            SetCurrentValue(ItemsSourceProperty, CreatePageItems(pageCount, normalizedPageIndex, DisplayPageCount));
            UpdateNavigationState();
            UpdatePseudoClasses();
        }

        private void UpdateCurrentRange()
        {
            if (PageCount == 0 || TotalItemCount <= 0 || PageSize <= 0)
            {
                StartItemIndex = 0;
                EndItemIndex = 0;
                StatusText = FormatText(TotalTextFormat, TotalItemCount);
                return;
            }

            var start = PageIndex * PageSize + 1;
            var end = Math.Min(start + PageSize - 1, TotalItemCount);
            StartItemIndex = start;
            EndItemIndex = end;
            StatusText = FormatText(StatusTextFormat, start, end, TotalItemCount);
        }

        private void SetCurrentPage(int pageIndex)
        {
            SetCurrentValue(PageIndexProperty, NormalizePageIndex(pageIndex, PageCount));
        }

        internal void SelectPage(int pageIndex)
        {
            if (pageIndex >= 0)
            {
                SetCurrentPage(pageIndex);
            }
        }

        private int GetJumpPageIndex(int pageIndex)
        {
            if (pageIndex == JumpPreviousPageIndex)
            {
                return NormalizePageIndex(PageIndex - DisplayPageCount, PageCount);
            }

            if (pageIndex == JumpNextPageIndex)
            {
                return NormalizePageIndex(PageIndex + DisplayPageCount, PageCount);
            }

            return NormalizePageIndex(pageIndex, PageCount);
        }

        /// <summary>
        /// Confirms the quick jump input.
        /// </summary>
        public void ConfirmQuickJump()
        {
            var text = _quickJumpBox?.Text;
            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out var pageNumber))
            {
                SetCurrentPage(pageNumber - 1);
                if (_quickJumpBox != null)
                {
                    _quickJumpBox.Text = string.Empty;
                }
            }
        }

        private void UpdateNavigationState()
        {
            CanGoPrevious = PageCount > 0 && PageIndex > 0;
            CanGoNext = PageCount > 0 && PageIndex < PageCount - 1;
            CanConfirmQuickJump = PageCount > 0;
        }

        private void OnQuickJumpBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && PageCount > 0)
            {
                ConfirmQuickJump();
                e.Handled = true;
            }
        }

        private void UpdatePseudoClasses()
        {
            PseudoClasses.Set(pcFirstPage, !CanGoPrevious);
            PseudoClasses.Set(pcLastPage, !CanGoNext);
            PseudoClasses.Set(pcEmpty, PageCount == 0);
        }

        private static int CalculatePageCount(int totalItemCount, int pageSize)
        {
            if (totalItemCount <= 0 || pageSize <= 0)
            {
                return 0;
            }

            return (totalItemCount - 1) / pageSize + 1;
        }

        private static int NormalizePageIndex(int pageIndex, int pageCount)
        {
            if (pageCount <= 0)
            {
                return 0;
            }

            return Math.Clamp(pageIndex, 0, pageCount - 1);
        }

        private static int NormalizePageIndex(int pageIndex, int pageCount, int oldPageCount)
        {
            if (pageCount > 0)
            {
                return NormalizePageIndex(pageIndex, pageCount);
            }

            return oldPageCount > 0 ? 0 : Math.Max(0, pageIndex);
        }

        private static List<int> CreatePageItems(int pageCount, int pageIndex, int displayPageCount)
        {
            if (pageCount <= 0)
            {
                return [];
            }

            var items = new List<int>();
            if (pageCount <= displayPageCount)
            {
                for (var i = 0; i < pageCount; i++)
                {
                    items.Add(i);
                }

                return items;
            }

            var sideWindowCount = displayPageCount - 2;
            if (pageIndex < sideWindowCount - 1)
            {
                for (var i = 0; i < sideWindowCount; i++)
                {
                    items.Add(i);
                }

                items.Add(JumpNextPageIndex);
                items.Add(pageCount - 1);
                return items;
            }

            if (pageIndex >= pageCount - sideWindowCount)
            {
                items.Add(0);
                items.Add(JumpPreviousPageIndex);

                for (var i = pageCount - sideWindowCount; i < pageCount; i++)
                {
                    items.Add(i);
                }

                return items;
            }

            var middleWindowCount = displayPageCount - 4;
            var start = pageIndex - middleWindowCount / 2;
            var end = start + middleWindowCount - 1;
            items.Add(0);
            items.Add(JumpPreviousPageIndex);
            for (var i = start; i <= end; i++)
            {
                items.Add(i);
            }
            items.Add(JumpNextPageIndex);
            items.Add(pageCount - 1);

            return items;
        }

        private static string FormatPageNumber(int pageIndex)
        {
            return (pageIndex + 1).ToString();
        }

        private static string FormatText(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return string.Empty;
            }

            try
            {
                return string.Format(CultureInfo.CurrentCulture, format, args);
            }
            catch (FormatException)
            {
                return format;
            }
        }
    }

    /// <summary>
    /// Represents a generated page button container used by <see cref="Pagination"/>.
    /// </summary>
    [PseudoClasses(pcCurrent, pcJumpPrevious, pcJumpNext)]
    public class PaginationItem : Button
    {
        private const string pcCurrent = ":current";
        private const string pcJumpPrevious = ":jump-previous";
        private const string pcJumpNext = ":jump-next";

        internal Pagination? Owner { get; set; }

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