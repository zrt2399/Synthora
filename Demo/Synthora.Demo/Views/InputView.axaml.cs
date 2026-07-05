using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Synthora.Controls;

namespace Synthora.Demo.Views
{
    public partial class InputView : UserControl
    {
        private int _directMultiComboBoxItemIndex = 1;

        public InputView()
        {
            InitializeComponent();
            SelectDefaultMultiComboBoxRoles();
        }

        private void AddDirectMultiComboBoxItem(object? sender, RoutedEventArgs e)
        {
            var item = new MultiComboBoxItem
            {
                Content = $"Custom module {_directMultiComboBoxItemIndex++}"
            };

            DirectMultiComboBox.Items.Add(item);
            //DirectMultiComboBox.SelectedItems?.Add(item);
        }

        private void SelectDefaultMultiComboBoxRoles()
        {
            DirectMultiComboBox.SelectedItems?.Clear();
            DirectMultiComboBox.SelectedItems?.Add(DirectCalendarItem);
            DirectMultiComboBox.SelectedItems?.Add(DirectDialogHostItem);
        }

        private void SelectDefaultMultiComboBoxRoles(object? sender, RoutedEventArgs e)
        {
            SelectDefaultMultiComboBoxRoles();
        }

        private void RemoveDirectMultiComboBoxSelectedItems(object? sender, RoutedEventArgs e)
        {
            var selectedItems = DirectMultiComboBox.SelectedItems?.Cast<object>().ToList();
            if (selectedItems == null)
            {
                return;
            }

            foreach (var item in selectedItems)
            {
                DirectMultiComboBox.SelectedItems?.Remove(item);
                //DirectMultiComboBox.Items.Remove(item);
            }
        }

        private void RemoveDirectMultiComboBoxLastItem(object? sender, RoutedEventArgs e)
        {
            if (DirectMultiComboBox.Items.Count > 0)
            {
                DirectMultiComboBox.Items.Remove(DirectMultiComboBox.Items[DirectMultiComboBox.Items.Count - 1]);
            }
        }
    }
}