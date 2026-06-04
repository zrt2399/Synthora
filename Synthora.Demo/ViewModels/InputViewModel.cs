using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class InputViewModel : TreeMenuDemoItem
    {
        [ObservableProperty]
        public partial ObservableCollection<string> AutoCompleteSuggestions { get; set; } =
        [
            "Avalonia",
            "AutoCompleteBox",
            "ButtonSpinner",
            "ComboBox",
            "Compiled Binding",
            "Control Theme",
            "DataGrid",
            "Density Style",
            "Fluent Theme",
            "Input Control",
            "Material Icons",
            "NumericUpDown",
            "SelectableTextBlock",
            "Synthora",
            "TextBox",
            "Theme Variant"
        ];

        [ObservableProperty]
        public partial string AutoCompleteText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Greeting { get; set; } = "Welcome to Avalonia!";

        [ObservableProperty]
        public partial ThemeMode SelectedEnumDescriptionMode { get; set; }

        [ObservableProperty]
        public partial double Value { get; set; }

        public InputViewModel()
        {
            IconKind = MaterialIconKind.FormTextbox;
            Description = "AutoCompleteBox, TextBox, ButtonSpinner, NumericUpDown, ComboBox";
        }
    }
}