using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Utilities;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a combo-box style list that supports multiple selected items.
    /// </summary>
    [PseudoClasses(pcDropDownOpen, pcPressed)]
    public class MultiComboBox : ListBox, IWeakEventSubscriber<NotifyCollectionChangedEventArgs>
    {
        private const string pcDropDownOpen = ":dropdownopen";
        private const string pcPressed = ":pressed";
        private const string defaultItemDisplayStringFormat = "{0}";

        private Popup? _popup;
        private TextBox? _textBox;
        private bool _updatingSelectedItems;
        private string? _selectedText;

        /// <summary>
        /// Defines the <see cref="IsDropDownOpen"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsDropDownOpenProperty =
            ComboBox.IsDropDownOpenProperty.AddOwner<MultiComboBox>();

        /// <summary>
        /// Defines the <see cref="MaxDropDownHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> MaxDropDownHeightProperty =
            ComboBox.MaxDropDownHeightProperty.AddOwner<MultiComboBox>();

        /// <summary>
        /// Defines the <see cref="PlaceholderText"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> PlaceholderTextProperty =
           TextBox.PlaceholderTextProperty.AddOwner<ComboBox>();

        /// <summary>
        /// Defines the <see cref="PlaceholderForeground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
            TextBox.PlaceholderForegroundProperty.AddOwner<ComboBox>();

        /// <summary>
        /// Defines the <see cref="HorizontalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            ContentControl.HorizontalContentAlignmentProperty.AddOwner<MultiComboBox>();

        /// <summary>
        /// Defines the <see cref="VerticalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            ContentControl.VerticalContentAlignmentProperty.AddOwner<MultiComboBox>();

        /// <summary>
        /// Defines the <see cref="TextWrapping"/> property.
        /// </summary>
        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            TextBox.TextWrappingProperty.AddOwner<MultiComboBox>();

        /// <summary>
        /// Defines the <see cref="AllowTextSelection"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> AllowTextSelectionProperty =
           AvaloniaProperty.Register<MultiComboBox, bool>(nameof(AllowTextSelection));

        /// <summary>
        /// Defines the <see cref="SelectedText"/> property.
        /// </summary>
        public static readonly DirectProperty<MultiComboBox, string?> SelectedTextProperty =
            AvaloniaProperty.RegisterDirect<MultiComboBox, string?>(nameof(SelectedText), o => o.SelectedText);

        /// <summary>
        /// Defines the <see cref="SelectionSeparator"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> SelectionSeparatorProperty =
            AvaloniaProperty.Register<MultiComboBox, string?>(nameof(SelectionSeparator), ", ");

        /// <summary>
        /// Defines the <see cref="ItemDisplayStringFormat"/> property.
        /// </summary>
        public static readonly StyledProperty<string> ItemDisplayStringFormatProperty =
            AvaloniaProperty.Register<MultiComboBox, string>(nameof(ItemDisplayStringFormat), defaultItemDisplayStringFormat);

        /// <summary>
        /// Defines the <see cref="ShowClearButton"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowClearButtonProperty =
            AvaloniaProperty.Register<MultiComboBox, bool>(nameof(ShowClearButton));

        /// <summary>
        /// Defines the <see cref="PopupTopContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PopupTopContentProperty =
            AvaloniaProperty.Register<MultiComboBox, object?>(nameof(PopupTopContent));

        /// <summary>
        /// Defines the <see cref="PopupBottomContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> PopupBottomContentProperty =
            AvaloniaProperty.Register<MultiComboBox, object?>(nameof(PopupBottomContent));

        /// <summary>
        /// Defines the <see cref="AllSelectedText"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> AllSelectedTextProperty =
            AvaloniaProperty.Register<MultiComboBox, string?>(nameof(AllSelectedText), "All selected");

        /// <summary>
        /// Gets or sets whether the drop-down is open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get => GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum height of the drop-down.
        /// </summary>
        public double MaxDropDownHeight
        {
            get => GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the placeholder text shown when there is no selection.
        /// </summary>
        public string? PlaceholderText
        {
            get => GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the placeholder foreground brush.
        /// </summary>
        public IBrush? PlaceholderForeground
        {
            get => GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the displayed text.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the displayed text.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets how the selected text wraps in the display box.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get => GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the displayed selected text can receive focus and be selected.
        /// </summary>
        public bool AllowTextSelection
        {
            get => GetValue(AllowTextSelectionProperty);
            set => SetValue(AllowTextSelectionProperty, value);
        }

        /// <summary>
        /// Gets the text generated from the currently selected items.
        /// </summary>
        public string? SelectedText
        {
            get => _selectedText;
            private set => SetAndRaise(SelectedTextProperty, ref _selectedText, value);
        }

        /// <summary>
        /// Gets or sets the separator used between selected item texts.
        /// </summary>
        public string? SelectionSeparator
        {
            get => GetValue(SelectionSeparatorProperty);
            set => SetValue(SelectionSeparatorProperty, value);
        }

        /// <summary>
        /// Gets or sets the format string used for each selected item in <see cref="SelectedText"/>.
        /// </summary>
        public string ItemDisplayStringFormat
        {
            get => GetValue(ItemDisplayStringFormatProperty);
            set => SetValue(ItemDisplayStringFormatProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the clear-selection button is shown.
        /// </summary>
        public bool ShowClearButton
        {
            get => GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed above the popup item list.
        /// </summary>
        public object? PopupTopContent
        {
            get => GetValue(PopupTopContentProperty);
            set => SetValue(PopupTopContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed below the popup item list.
        /// </summary>
        public object? PopupBottomContent
        {
            get => GetValue(PopupBottomContentProperty);
            set => SetValue(PopupBottomContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the text shown when all items are selected.
        /// </summary>
        public string? AllSelectedText
        {
            get => GetValue(AllSelectedTextProperty);
            set => SetValue(AllSelectedTextProperty, value);
        }

        static MultiComboBox()
        {
            SelectionModeProperty.OverrideDefaultValue<MultiComboBox>(SelectionMode.Multiple | SelectionMode.Toggle);
        }

        /// <summary>
        /// Occurs after the drop-down (popup) list of the <see cref="MultiComboBox"/> closes.
        /// </summary>
        public event EventHandler? DropDownClosed;

        /// <summary>
        /// Occurs after the drop-down (popup) list of the <see cref="MultiComboBox"/> opens.
        /// </summary>
        public event EventHandler? DropDownOpened;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiComboBox"/> class.
        /// </summary>
        public MultiComboBox()
        {
            SelectionChanged += OnSelectionChanged;
            if (Items is INotifyCollectionChanged notifyCollectionChanged)
            {
                WeakEvents.CollectionChanged.Subscribe(notifyCollectionChanged, this);
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (_popup != null)
            {
                _popup.Opened -= OnPopupOpened;
                _popup.Closed -= OnPopupClosed;
            }

            base.OnApplyTemplate(e);

            _popup = e.NameScope.Find<Popup>("PART_Popup");
            _textBox = e.NameScope.Find<TextBox>("PART_TextBox");

            if (_popup != null)
            {
                _popup.Opened += OnPopupOpened;
                _popup.Closed += OnPopupClosed;
            }

            UpdateDropDownPseudoClass();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            PseudoClasses.Set(pcPressed, true);
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (!IsDropDownOpen && (!AllowTextSelection || e.Source is not StyledElement { TemplatedParent: var parent } || parent != _textBox))
            {
                SetCurrentValue(IsDropDownOpenProperty, true);
            }

            PseudoClasses.Set(pcPressed, false);
            base.OnPointerReleased(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            if (!e.Handled && IsDropDownOpen)
            {
                e.Handled = true;
            }
        }

        protected override void OnGotFocus(FocusChangedEventArgs e)
        {
            if (!e.Handled && _textBox != null)
            {
                if (Equals(e.Source, this) && AllowTextSelection)
                {
                    _textBox.Focus();
                    _textBox.SelectAll();
                }
            }

            base.OnGotFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if ((e.Key == Key.F4 && e.KeyModifiers.HasFlag(KeyModifiers.Alt) == false) ||
                    ((e.Key == Key.Down || e.Key == Key.Up) && e.KeyModifiers.HasFlag(KeyModifiers.Alt)))
                {
                    SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
                    e.Handled = true;
                }
                else if (IsDropDownOpen && e.Key == Key.Escape)
                {
                    SetCurrentValue(IsDropDownOpenProperty, false);
                    e.Handled = true;
                }
                else if (!IsDropDownOpen && !AllowTextSelection && (e.Key == Key.Enter || e.Key == Key.Space))
                {
                    SetCurrentValue(IsDropDownOpenProperty, true);
                    e.Handled = true;
                }
                else if (IsDropDownOpen && e.Key == Key.Tab)
                {
                    SetCurrentValue(IsDropDownOpenProperty, false);
                }
                if (e.Handled)
                {
                    return;
                }
            }

            base.OnKeyDown(e);
        }

        protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        {
            return new MultiComboBoxItem();
        }

        protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
        {
            return NeedsContainer<MultiComboBoxItem>(item, out recycleKey);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == IsDropDownOpenProperty)
            {
                UpdateDropDownPseudoClass();
            }
            else if (change.Property == SelectedItemsProperty)
            {
                UpdateSelectedText();
                SubscribeCollection(change.OldValue, change.NewValue);
            }
            else if (change.Property == SelectionSeparatorProperty || change.Property == AllSelectedTextProperty)
            {
                UpdateSelectedText();
            }
            else if (change.Property == ItemsSourceProperty)
            {
                UpdateSelectedText();
            }
        }

        /// <summary>
        /// Inverts the current selection.
        /// </summary>
        public void InvertSelection()
        {
            var selectedItems = SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                SelectAll();
                return;
            }
            if (selectedItems.Count == Items.Count)
            {
                UnselectAll();
                return;
            }

            _updatingSelectedItems = true;
            try
            {
                var itemCount = Items.Count;
                var selectedIndexes = Selection.SelectedIndexes.ToHashSet();

                Selection.Clear();

                for (var i = 0; i < itemCount; i++)
                {
                    if (!selectedIndexes.Contains(i))
                    {
                        Selection.Select(i);
                    }
                }
            }
            finally
            {
                _updatingSelectedItems = false;
            }

            UpdateSelectedText();
        }

        internal void ItemFocused(MultiComboBoxItem dropDownItem)
        {
            if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid)
            {
                dropDownItem.BringIntoView();
            }
        }

        private void OnPopupOpened(object? sender, EventArgs e)
        {
            TryFocusSelectedItem();
            DropDownOpened?.Invoke(this, EventArgs.Empty);
        }

        private void OnPopupClosed(object? sender, EventArgs e)
        {
            if (AllowTextSelection && CanFocus(this))
            {
                Focus();
            }

            DropDownClosed?.Invoke(this, EventArgs.Empty);
        }

        private void TryFocusSelectedItem()
        {
            var selectedIndex = SelectedIndex;
            if (IsDropDownOpen && selectedIndex != -1)
            {
                ScrollIntoView(selectedIndex);
                var container = ContainerFromIndex(selectedIndex);

                if (container != null && CanFocus(container))
                {
                    container.Focus();
                }
            }
        }

        private bool CanFocus(Control control) => control.Focusable && control.IsEffectivelyEnabled && control.IsVisible;

        private void SubscribeCollection(object? oldValue, object? newValue)
        {
            if (oldValue is INotifyCollectionChanged oldNotifyCollectionChanged)
            {
                WeakEvents.CollectionChanged.Unsubscribe(oldNotifyCollectionChanged, this);
            }
            if (newValue is INotifyCollectionChanged newNotifyCollectionChanged)
            {
                WeakEvents.CollectionChanged.Subscribe(newNotifyCollectionChanged, this);
            }
        }

        void IWeakEventSubscriber<NotifyCollectionChangedEventArgs>.OnEvent(object? sender, WeakEvent ev, NotifyCollectionChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, SelectedItems))
            {
                UpdateSelectedText();
            }
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedText();
        }

        private void UpdateSelectedText()
        {
            if (_updatingSelectedItems)
            {
                return;
            }

            var selectedItems = SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                SelectedText = string.Empty;
                return;
            }

            var allSelectedText = AllSelectedText;
            if (selectedItems.Count == Items.Count && !string.IsNullOrEmpty(allSelectedText))
            {
                SelectedText = allSelectedText;
                return;
            }

            var format = ItemDisplayStringFormat;
            var useDefaultFormat = format == defaultItemDisplayStringFormat;
            var separator = SelectionSeparator;
            var stringBuilder = new StringBuilder();
            var appendSeparator = false;
            foreach (var item in selectedItems)
            {
                if (appendSeparator && !string.IsNullOrEmpty(separator))
                {
                    stringBuilder.Append(separator);
                }

                stringBuilder.Append(FormatSelectedItem(item, format, useDefaultFormat));
                appendSeparator = true;
            }

            SelectedText = stringBuilder.ToString();
        }

        private static string FormatSelectedItem(object? item, string format, bool useDefaultFormat)
        {
            var value = item is ContentControl contentControl ? contentControl.Content : item;

            return useDefaultFormat
                ? value?.ToString() ?? string.Empty
                : string.Format(format, value);
        }

        private void UpdateDropDownPseudoClass()
        {
            PseudoClasses.Set(pcDropDownOpen, IsDropDownOpen);
        }
    }
}