using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class InputViewModel : TreeMenuDemoItem
    {
        private int _customRoleIndex = 1;

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
        public partial IFontCollection SystemFonts { get; set; } = FontManager.Current.SystemFonts;

        [ObservableProperty]
        public partial string AutoCompleteText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Greeting { get; set; } = "Welcome to Avalonia!";

        [ObservableProperty]
        public partial ThemeMode SelectedEnumDescriptionMode { get; set; }

        [ObservableProperty]
        public partial double Value { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MultiComboBoxRole> MultiComboBoxRoles { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<MultiComboBoxRole> SelectedMultiComboBoxRoles { get; set; } = [];

        public InputViewModel()
        {
            IconKind = MaterialIconKind.FormTextbox;
            Description = "AutoCompleteBox, TextBox, ButtonSpinner, NumericUpDown, ComboBox, MultiComboBox";
            ResetMultiComboBoxRoles();
        }

        public void AddMultiComboBoxRole()
        {
            var role = new MultiComboBoxRole($"Custom role {_customRoleIndex++}", "Custom");
            MultiComboBoxRoles.Add(role);
            //SelectedMultiComboBoxRoles.Add(role);
        }

        public void RemoveSelectedMultiComboBoxRoles()
        {
            var selectedRoles = SelectedMultiComboBoxRoles.ToList();
            foreach (var role in selectedRoles)
            {
                //MultiComboBoxRoles.Remove(role);
                SelectedMultiComboBoxRoles.Remove(role);
            }
        }

        public void SelectDefaultMultiComboBoxRoles()
        {
            SelectedMultiComboBoxRoles.Clear();
            foreach (var role in MultiComboBoxRoles.Where(x => x.Name is "Designer" or "Reviewer"))
            {
                SelectedMultiComboBoxRoles.Add(role);
            }
        }

        public void RemoveMultiComboBoxRolesLastItem()
        {
            if (SelectedMultiComboBoxRoles.Count > 0)
            {
                MultiComboBoxRoles.RemoveAt(MultiComboBoxRoles.Count - 1);
            }
        }

        public void ResetMultiComboBoxRoles()
        {
            MultiComboBoxRoles =
            [
                new("Designer", "Product"),
                new("Developer", "Engineering"),
                new("Reviewer", "Engineering"),
                new("Tester", "Quality"),
                new("Publisher", "Release"),
                new("Support", "Operations")
            ];

            SelectedMultiComboBoxRoles = [MultiComboBoxRoles[0], MultiComboBoxRoles[2]];
        }
    }

    public class MultiComboBoxRole
    {
        public MultiComboBoxRole(string name, string group)
        {
            Name = name;
            Group = group;
        }

        public string Name { get; }

        public string Group { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}