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
            Description = "DragTabControl, DragTabItem";

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
            Description = "TabControl, TabItem";
        }
    }

    public class SplitViewViewModel : TreeMenuDemoItem
    {
        public SplitViewViewModel()
        {
            IconKind = MaterialIconKind.ViewSplitVertical;
            Description = "SplitView, TabStrip, TabStripItem";
        }
    }

    public class TabbedPageViewModel : TreeMenuDemoItem
    {
        public TabbedPageViewModel()
        {
            IconKind = MaterialIconKind.TabPlus;
            Description = "TabbedPage, ContentPage";
        }
    }

    public class DrawerPageViewModel : TreeMenuDemoItem
    {
        public DrawerPageViewModel()
        {
            IconKind = MaterialIconKind.DockLeft;
            Description = "DrawerPage";
        }
    }

    public class NavigationPageViewModel : TreeMenuDemoItem
    {
        public NavigationPageViewModel()
        {
            IconKind = MaterialIconKind.NavigationVariant;
            Description = "NavigationPage";
        }
    }

    public class CarouselViewModel : TreeMenuDemoItem
    {
        public CarouselViewModel()
        {
            IconKind = MaterialIconKind.ViewCarousel;
            Description = "Carousel, PipsPager, CarouselPage, ContentPage";
        }
    }

    public class TreeMenuViewModel : TreeMenuDemoItem
    {
        public TreeMenuViewModel()
        {
            IconKind = MaterialIconKind.WidgetTree;
            Description = "TreeMenu, TreeMenuItem";
        }
    }
}