using Avalonia.Controls;

namespace Synthora.Demo.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        dataGrid.LoadingRow += DataGrid_LoadingRow;
    }

    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = e.Row.Index + 1;
    }
}