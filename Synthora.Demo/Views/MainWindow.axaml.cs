using Avalonia.Controls;
using Avalonia.Controls.Primitives;

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
    }
}