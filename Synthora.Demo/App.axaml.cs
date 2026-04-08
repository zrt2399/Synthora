using System;
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
        public static MainWindow MainWindow
        {
            get
            {
                if (((IClassicDesktopStyleApplicationLifetime?)Current?.ApplicationLifetime)?.MainWindow is not MainWindow mainWindow)
                {
                    throw new Exception("MainWindow is not available.");
                }
                return mainWindow;
            }
        }

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
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}