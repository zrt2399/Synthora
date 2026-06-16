using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace Synthora.Demo.Models
{
    public partial class DragTabItemModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;
        
        [ObservableProperty] 
        public partial MaterialIconKind IconKind { get; set; }
    }
}