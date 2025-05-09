using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Synthora.Demo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.LoadingRow += DataGrid_LoadingRow;
        }

        private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.Index + 1;
        }

        private void Button_Click(object? sender, RoutedEventArgs e)
        { 
            testTextBox.SetCurrentValue(TextBox.TextProperty, "Button clicked!");
            //testTextBox.SetValue( TextBox.TextProperty, "Button clicked!"); 
        }
    }
}