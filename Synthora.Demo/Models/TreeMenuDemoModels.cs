using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace Synthora.Demo.Models
{
    public partial class TreeMenuDemoItem : ObservableObject
    {
        public TreeMenuDemoItem()
        {
            Title = GetType().Name.Replace("ViewModel", "");
        }
        
        [ObservableProperty]
        public partial MaterialIconKind IconKind { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; }

        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ObservableCollection<TreeMenuDemoItem> Children { get; set; } = [];

        [ObservableProperty]
        public partial bool IsSelected { get; set; }

        [ObservableProperty]
        public partial bool IsExpanded { get; set; }
    }
}