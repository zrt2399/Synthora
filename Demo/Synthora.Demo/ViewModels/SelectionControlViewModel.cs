using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class SelectionControlViewModel : TreeMenuDemoItem
    {
        public SelectionControlViewModel()
        {
            IconKind = MaterialIconKind.Selection;
            Description = "CheckBox, RadioButton, ToggleSwitch";
        }
    }
}