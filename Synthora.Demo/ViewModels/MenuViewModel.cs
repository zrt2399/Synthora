using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class MenuViewModel: TreeMenuDemoItem
    {
        public MenuViewModel()
        {
            IconKind = MaterialIconKind.HamburgerMenu;
            Description = "Menu, MenuItem, MenuFlyout, ContextMenu, ToolTip, CommandBar, CommandBarButton, CommandBarToggleButton";
        } 
    }
}