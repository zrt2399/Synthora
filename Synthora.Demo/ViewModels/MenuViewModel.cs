using System.Collections.Generic;
using System.Collections.ObjectModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class MenuViewModel : TreeMenuDemoItem
    {
        public MenuViewModel()
        {
            IconKind = MaterialIconKind.HamburgerMenu;
            Description = "Menu, MenuItem, MenuFlyout, ContextMenu, ToolTip, CommandBar, CommandBarButton, CommandBarToggleButton";

            var items = new List<string>();
            for (int i = 1; i <= 80; i++)
            {
                items.Add($"Menu Item {i}");
            }
            MenuItems = new ObservableCollection<string>(items);
        }

        public ObservableCollection<string> MenuItems { get; }
    }
}