using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Synthora.Demo.Views
{
    public partial class NavigationPageView : UserControl
    {
        public NavigationPageView()
        {
            InitializeComponent(); 
            navigationPage.PushAsync(new ContentPage() { Header = "Home", Content = new TextBlock() { Text = "HomePage" } });
        }
         
        private void PushAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PushAsync(new ContentPage() { Header = "PushAsyncHeader", Content = new TextBlock() { Text = "PushAsyncContent" } });
        }
        private void PopAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopAsync();
        }
        private void PopToRootAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopToRootAsync();
        }

        private void PushModalAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PushModalAsync(new ContentPage() { Header = "PushAsyncHeader", Content = new TextBlock() { Text = "PushModalAsyncContent" } });
        }
        private void PopModalAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopModalAsync();
        }
        private void PopAllModalsAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopAllModalsAsync();
        }
    }
}