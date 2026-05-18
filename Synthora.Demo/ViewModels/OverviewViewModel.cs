using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class OverviewViewModel : TreeMenuDemoItem
    {
        public OverviewViewModel()
        {
            IconKind = MaterialIconKind.ViewDashboard;
        }
    }
}