using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class NavigationViewModel : TreeMenuDemoItem
    {
        public NavigationViewModel()
        {
            IconKind = MaterialIconKind.Compass;
        }
    }

    public partial class DragTabControlViewModel : TreeMenuDemoItem
    {
        private int _i;
        private static readonly MaterialIconKind[] IconKinds = Enum.GetValues<MaterialIconKind>();

        public DragTabControlViewModel()
        {
            IconKind = MaterialIconKind.DragVariant;

            var list = new List<DragTabItemModel>();
            for (var i = 0; i < 5; i++)
            {
                list.Add(AddItem());
            }
            TabItems = new ObservableCollection<DragTabItemModel>(list);
        }

        public Func<object> NewItemFactory => AddItem;

        [ObservableProperty]
        public partial ObservableCollection<DragTabItemModel> TabItems { get; set; }

        private DragTabItemModel AddItem()
        {
            return new DragTabItemModel
            {
                Title = $"Tab {++_i}",
                IconKind = GetRandomIconKind()
            };
        }

        private static MaterialIconKind GetRandomIconKind()
        {
            return IconKinds[Random.Shared.Next(IconKinds.Length)];
        }
    }

    public class TabControlViewModel : TreeMenuDemoItem
    {
        public TabControlViewModel()
        {
            IconKind = MaterialIconKind.Tab;
        }
    }

    public class SplitViewViewModel : TreeMenuDemoItem
    {
        public SplitViewViewModel()
        {
            IconKind = MaterialIconKind.ViewSplitVertical;
        }
    }

    public class TabbedPageViewModel : TreeMenuDemoItem
    {
        public TabbedPageViewModel()
        {
            IconKind = MaterialIconKind.TabPlus;
        }
    }

    public class DrawerPageViewModel : TreeMenuDemoItem
    {
        public DrawerPageViewModel()
        {
            IconKind = MaterialIconKind.DockLeft;
        }
    }

    public class NavigationPageViewModel : TreeMenuDemoItem
    {
        public NavigationPageViewModel()
        {
            IconKind = MaterialIconKind.NavigationVariant;
        }
    }

    public class TreeMenuViewModel : TreeMenuDemoItem
    {
        public TreeMenuViewModel()
        {
            IconKind = MaterialIconKind.WidgetTree;
        }
    }
}