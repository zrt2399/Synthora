using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class ExpanderViewModel: TreeMenuDemoItem
    {
        public ExpanderViewModel()
        {
            IconKind = MaterialIconKind.ExpansionCard;
            Description = "Expander";
        } 
    }
}