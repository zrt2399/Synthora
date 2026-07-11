using Avalonia;
using Avalonia.Controls;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="MultiComboBox"/>.
    /// </summary>
    public class MultiComboBoxItem : ListBoxItem
    {
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == IsFocusedProperty && change.GetNewValue<bool>())
            {
                (Parent as MultiComboBox)?.ItemFocused(this);
            }
        }
    }
}