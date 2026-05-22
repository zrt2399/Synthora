using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class InputViewModel : TreeMenuDemoItem
    {
        [ObservableProperty]
        public partial string Greeting { get; set; } = "Welcome to Avalonia!";

        [ObservableProperty]
        public partial ThemeMode SelectedEnumDescriptionMode { get; set; }

        [ObservableProperty]
        public partial double Value { get; set; }

        public InputViewModel()
        {
            IconKind = MaterialIconKind.FormTextbox;
            Description = "AutoCompleteBox, TextBox, TextBlock, SelectableTextBlock, ButtonSpinner, NumericUpDown, ComboBox";
        }
    }
}