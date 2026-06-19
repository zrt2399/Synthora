using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class ButtonsViewModel : TreeMenuDemoItem
    {
        public ButtonsViewModel()
        {
            IconKind = MaterialIconKind.ButtonPointer;
            Description = "Button, RepeatButton, ToggleButton, DropDownButton, SplitButton, ToggleSplitButton, HyperlinkButton";
        }
    }
}