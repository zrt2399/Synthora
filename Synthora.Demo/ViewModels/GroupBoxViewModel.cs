using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class GroupBoxViewModel : TreeMenuDemoItem
    {
        public GroupBoxViewModel()
        {
            IconKind = MaterialIconKind.ApplicationOutline;
            Description = "GroupBox, GroupBoxEx, DropShadowChrome";
        }
    }
}