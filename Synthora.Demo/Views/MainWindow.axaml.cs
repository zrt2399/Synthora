using Avalonia.Controls;
using PropertyChanged;

namespace Synthora.Demo.Views
{
    [DoNotNotify]
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
    }
}