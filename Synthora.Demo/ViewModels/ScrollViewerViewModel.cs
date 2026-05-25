using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class ScrollViewerViewModel: TreeMenuDemoItem
    {
        public ScrollViewerViewModel()
        {
            IconKind = MaterialIconKind.ArrowUpDownBold;
            Description = "ScrollBar, ScrollViewer";
        }
    }
}