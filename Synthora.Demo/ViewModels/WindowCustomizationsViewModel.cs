using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class WindowCustomizationsViewModel: TreeMenuDemoItem
    {
        public WindowCustomizationsViewModel()
        {
            IconKind = MaterialIconKind.ApplicationCog;
            Description = "Window";
        }
    }
}