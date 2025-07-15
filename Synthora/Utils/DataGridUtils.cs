using System.Windows.Input;
using Avalonia.Controls;
using Synthora.Commands;

namespace Synthora.Utils
{
    internal class DataGridUtils
    {
        public static ICommand SelectAllCommand { get; } = new RelayCommand<DataGrid?>(SelectAll);

        public static void SelectAll(DataGrid? dataGrid)
        {
            dataGrid?.SelectAll();
        }
    }
}