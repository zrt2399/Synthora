using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class BannerViewModel : TreeMenuDemoItem
    {
        public BannerViewModel()
        {
            IconKind = MaterialIconKind.AlertCircle;
            Description = "Banner";
        }
    }
}