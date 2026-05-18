using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class ButtonViewModel : TreeMenuDemoItem
    {
        public ButtonViewModel()
        {
            IconKind = MaterialIconKind.ButtonPointer;
        }
    }
}