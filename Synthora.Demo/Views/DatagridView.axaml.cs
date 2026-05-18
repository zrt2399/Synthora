using Avalonia.Controls;

namespace Synthora.Demo.Views
{
    public partial class DataGridView : UserControl
    {
        public DataGridView()
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