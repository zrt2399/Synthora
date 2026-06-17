using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class SelectionControlsViewModel : TreeMenuDemoItem
    {
        public SelectionControlsViewModel()
        {
            IconKind = MaterialIconKind.Selection;
            Description = "CheckBox, RadioButton, ToggleSwitch";
        }
    }
}