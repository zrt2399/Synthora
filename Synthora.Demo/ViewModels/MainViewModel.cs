using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                SelectedThemeMode = ToAppThemeMode(application.RequestedThemeVariant);
            }
            SelectedDensityStyle = SynthoraTheme.GetCurrentDensity();
            InitializeTreeMenuDemo();
        }

        [ObservableProperty]
        public partial ObservableCollection<TreeMenuDemoItem>? TreeMenuItems { get; set; }

        [ObservableProperty]
        public partial TreeMenuDemoItem? SelectedTreeMenuItem { get; set; }

        [ObservableProperty]
        public partial AppThemeMode SelectedThemeMode { get; set; }
        partial void OnSelectedThemeModeChanged(AppThemeMode value)
        {
            if (Application.Current is { } application)
            {
                application.RequestedThemeVariant = ToThemeVariant(value);
            }
        }

        [ObservableProperty]
        public partial DensityStyle SelectedDensityStyle { get; set; }
        partial void OnSelectedDensityStyleChanged(DensityStyle value)
        {
            SynthoraTheme.SetDensity(value);
        }

        public static Uri GitHubDepositoryUri { get; } = new Uri("https://github.com/zrt2399/Synthora");

        private static T CreateItem<T>(T item, params TreeMenuDemoItem[] children) where T : TreeMenuDemoItem
        {
            item.Children = new ObservableCollection<TreeMenuDemoItem>(children);
            return item;
        }

        private void InitializeTreeMenuDemo()
        {
            TreeMenuItems = new ObservableCollection<TreeMenuDemoItem>
            {
                CreateItem(new OverviewViewModel()
                {
                    IsSelected = true
                }),
                CreateItem(new BrushViewModel()),
                CreateItem(new InputViewModel()),
                CreateItem(new ButtonViewModel()),
                CreateItem(new SelectionControlViewModel()),
                CreateItem(new CollectionControlViewModel(), new DataGridViewModel(), new TreeViewViewModel(), new ListBoxViewModel()),
                CreateItem(new MenuViewModel()),
                CreateItem(new OverlayViewModel()),
                CreateItem(new CalendarViewModel()),
                CreateItem(new ExpanderViewModel()),
                CreateItem(new RangeControlViewModel()),
                CreateItem(new GroupBoxViewModel()),
                CreateItem(new ExtendedControlViewModel()),
                CreateItem(new NavigationViewModel(), new SplitViewViewModel(), new TabControlViewModel(), new TabbedPageViewModel(), new DrawerPageViewModel(), new NavigationPageViewModel(), new DragTabControlViewModel(), new TreeMenuViewModel())
            };
        }

        private static AppThemeMode ToAppThemeMode(ThemeVariant? themeVariant)
        {
            if (themeVariant == ThemeVariant.Default)
            {
                return AppThemeMode.Default;
            }
            return themeVariant == ThemeVariant.Light ? AppThemeMode.Light : AppThemeMode.Dark;
        }

        private static ThemeVariant ToThemeVariant(AppThemeMode mode)
        {
            return mode switch
            {
                AppThemeMode.Light => ThemeVariant.Light,
                AppThemeMode.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }

        public static ICommand ShowMainWindowCommand { get; } = new RelayCommand(() =>
        {
            if (!App.MainWindow.IsVisible)
            {
                App.MainWindow.Show();
            }
            if (App.MainWindow.WindowState == WindowState.Minimized)
            {
                App.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                App.MainWindow.Activate();
            }
        });

        public static ICommand OpenGitHubCommand { get; } = new RelayCommand(() => Process.Start(new ProcessStartInfo
        {
            FileName = GitHubDepositoryUri.ToString(),
            UseShellExecute = true
        }));
    }

    public enum AppThemeMode
    {
        [Description("系统")]
        Default,
        [Description("浅色")]
        Light,
        [Description("深色")]
        Dark
    }
}