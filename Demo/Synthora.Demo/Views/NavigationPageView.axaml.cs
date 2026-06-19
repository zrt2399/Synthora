using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace Synthora.Demo.Views
{
    public partial class NavigationPageView : UserControl
    {
        private static readonly (IBrush Background, IBrush Foreground)[] PagePalette =
        [
            (SolidColorBrush.Parse("#DCEBFF"), SolidColorBrush.Parse("#102A43")),
            (SolidColorBrush.Parse("#DDF6E8"), SolidColorBrush.Parse("#123524")),
            (SolidColorBrush.Parse("#FFE7CC"), SolidColorBrush.Parse("#5C3410")),
            (SolidColorBrush.Parse("#E9DDFB"), SolidColorBrush.Parse("#3E215B")),
            (SolidColorBrush.Parse("#FFDDE3"), SolidColorBrush.Parse("#5A1F2A"))
        ];

        private int _pageIndex;
        private int _modalIndex;

        public NavigationPageView()
        {
            InitializeComponent();
            NavigationPage.PushAsync(CreateHomePage());
        }

        private void PushAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PushAsync(CreatePage(++_pageIndex, "Page"));
        }

        private void PopAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PopAsync();
        }

        private void PopToRootAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PopToRootAsync();
            _pageIndex = 0;
        }

        private void PushModalAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PushModalAsync(CreatePage(++_modalIndex, "Modal"));
        }

        private void PopModalAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PopModalAsync();
        }

        private void PopAllModalsAsync(object? sender, RoutedEventArgs e)
        {
            NavigationPage.PopAllModalsAsync();
            _modalIndex = 0;
        }

        private static ContentPage CreateHomePage()
        {
            return new ContentPage
            {
                Header = "Home",
                Content = CreateContent("Root page", PagePalette[1])
            };
        }

        private static ContentPage CreatePage(int pageIndex, string header)
        {
            var palette = PagePalette[(pageIndex - 1) % PagePalette.Length];

            return new ContentPage
            {
                Header = $"{header} {pageIndex}",
                Content = CreateContent($"{header} {pageIndex}", palette)
            };
        }

        private static Border CreateContent(string title, (IBrush Background, IBrush Foreground) palette)
        {
            return new Border
            {
                Background = palette.Background,
                Child = new TextBlock
                {
                    Text = title,
                    FontSize = 28,
                    FontWeight = FontWeight.SemiBold,
                    Foreground = palette.Foreground,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
            };
        }
    }
}