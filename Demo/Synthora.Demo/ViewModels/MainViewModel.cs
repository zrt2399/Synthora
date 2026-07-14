using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            if (Application.Current is { } application)
            {
                SelectedThemeMode = ToThemeMode(application.RequestedThemeVariant);
            }
            SelectedThemeDensity = SynthoraTheme.Current?.ThemeDensity ?? ThemeDensity.Normal;
            InitializeTreeMenuDemo();
        }

        [ObservableProperty]
        public partial ObservableCollection<TreeMenuDemoItem>? TreeMenuItems { get; set; }

        [ObservableProperty]
        public partial TreeMenuDemoItem? SelectedTreeMenuItem { get; set; }

        [ObservableProperty]
        public partial string SearchText { get; set; } = string.Empty;
        partial void OnSearchTextChanged(string value) => FilterTreeMenu(value);

        [ObservableProperty]
        public partial ThemeMode SelectedThemeMode { get; set; }
        partial void OnSelectedThemeModeChanged(ThemeMode value)
        {
            if (Application.Current is { } application)
            {
                application.RequestedThemeVariant = ToThemeVariant(value);
            }
        }

        [ObservableProperty]
        public partial ThemeDensity SelectedThemeDensity { get; set; }
        partial void OnSelectedThemeDensityChanged(ThemeDensity value) => SynthoraTheme.Current?.ThemeDensity = value;

        public static Uri GitHubDepositoryUri { get; } = new Uri("https://github.com/zrt2399/Synthora");

        private static T CreateItem<T>(T item, params TreeMenuDemoItem[] children) where T : TreeMenuDemoItem
        {
            item.Children = new ObservableCollection<TreeMenuDemoItem>(children.OrderBy(static x => x.Title, StringComparer.OrdinalIgnoreCase));
            return item;
        }

        private void InitializeTreeMenuDemo()
        {
            TreeMenuItems = new ObservableCollection<TreeMenuDemoItem>(new TreeMenuDemoItem[]
            {
                CreateItem(new OverviewViewModel()
                {
                    IsSelected = true
                }),
                CreateItem(new BrushesViewModel()),
                CreateItem(new InputViewModel()),
                CreateItem(new TextBlockViewModel()),
                CreateItem(new ButtonsViewModel()),
                CreateItem(new SelectionControlsViewModel()),
                CreateItem(new CollectionControlsViewModel(), new DataGridViewModel(), new TableViewViewModel(), new TreeViewViewModel(), new ListBoxViewModel(), new RefreshContainerViewModel()),
                CreateItem(new MenuViewModel()),
                CreateItem(new BannerViewModel()),
                CreateItem(new OverlayViewModel()),
                CreateItem(new CalendarViewModel()),
                CreateItem(new ExpanderViewModel()),
                CreateItem(new ScrollViewerViewModel()),
                CreateItem(new RangeControlViewModel()),
                CreateItem(new GroupBoxViewModel()),
                CreateItem(new ExtendedControlsViewModel()),
                CreateItem(new ThemeVariantScopeViewModel()),
                CreateItem(new NavigationViewModel(), new SplitViewViewModel(), new TabControlViewModel(), new TabbedPageViewModel(), new DrawerPageViewModel(), new NavigationPageViewModel(), new CarouselViewModel(), new DragTabControlViewModel(), new TreeMenuViewModel()),
                CreateItem(new WindowCustomizationsViewModel())
            }.OrderByDescending(static x => x.IsSelected).ThenBy(static x => x.Title, StringComparer.OrdinalIgnoreCase));
        }

        private void FilterTreeMenu(string? searchText)
        {
            if (TreeMenuItems is null)
            {
                return;
            }

            var keyword = searchText?.Trim();
            foreach (var item in TreeMenuItems)
            {
                ApplyFilter(item, keyword);
            }
        }

        private static bool ApplyFilter(TreeMenuDemoItem item, string? keyword)
        {
            var hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            var selfMatched = !hasKeyword
                              || item.Title.Contains(keyword!, StringComparison.OrdinalIgnoreCase)
                              || item.Description.Contains(keyword!, StringComparison.OrdinalIgnoreCase);
            var childMatched = false;

            foreach (var child in item.Children)
            {
                childMatched |= ApplyFilter(child, keyword);
            }

            item.IsVisible = selfMatched || childMatched;
            item.IsExpanded = hasKeyword && childMatched;
            return item.IsVisible;
        }

        private static ThemeMode ToThemeMode(ThemeVariant? themeVariant)
        {
            if (themeVariant == ThemeVariant.Default)
            {
                return ThemeMode.Default;
            }
            return themeVariant == ThemeVariant.Light ? ThemeMode.Light : ThemeMode.Dark;
        }

        private static ThemeVariant ToThemeVariant(ThemeMode mode)
        {
            return mode switch
            {
                ThemeMode.Light => ThemeVariant.Light,
                ThemeMode.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }

        public static ICommand ShowMainWindowCommand { get; } = new RelayCommand(() =>
        {
            if (App.MainWindow is Window window)
            {
                if (!window.IsVisible)
                {
                    window.Show();
                }
                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.Activate();
                }
            }
        });

        public static ICommand OpenGitHubCommand { get; } = new RelayCommand(() => Process.Start(new ProcessStartInfo
        {
            FileName = GitHubDepositoryUri.ToString(),
            UseShellExecute = true
        }));
    }
}