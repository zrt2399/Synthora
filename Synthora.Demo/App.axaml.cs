using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Synthora.Demo.ViewModels;
using Synthora.Demo.Views;

namespace Synthora.Demo
{
    public partial class App : Application
    {
        public static MainWindow? MainWindow => Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime { MainWindow: MainWindow { PlatformImpl: not null } mainWindow }
            ? mainWindow
            : null;

        public static string AppVersion => $"{Assembly.GetExecutingAssembly().GetName().Version}";

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}