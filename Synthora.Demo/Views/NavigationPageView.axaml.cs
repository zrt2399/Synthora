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
            (new SolidColorBrush(Color.Parse("#DCEBFF")), new SolidColorBrush(Color.Parse("#102A43"))),
            (new SolidColorBrush(Color.Parse("#DDF6E8")), new SolidColorBrush(Color.Parse("#123524"))),
            (new SolidColorBrush(Color.Parse("#FFE7CC")), new SolidColorBrush(Color.Parse("#5C3410"))),
            (new SolidColorBrush(Color.Parse("#E9DDFB")), new SolidColorBrush(Color.Parse("#3E215B"))),
            (new SolidColorBrush(Color.Parse("#FFDDE3")), new SolidColorBrush(Color.Parse("#5A1F2A")))
        ];

        private int _pageIndex;
        private int _modalIndex;

        public NavigationPageView()
        {
            InitializeComponent();
            navigationPage.PushAsync(CreateHomePage());
        }

        private void PushAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PushAsync(CreatePage(++_pageIndex, "Page", "PushAsyncContent"));
        }

        private void PopAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopAsync();
        }

        private void PopToRootAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopToRootAsync();
            _pageIndex = 0;
        }

        private void PushModalAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PushModalAsync(CreatePage(++_modalIndex, "Modal", "PushModalAsyncContent"));
        }

        private void PopModalAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopModalAsync();
        }

        private void PopAllModalsAsync(object? sender, RoutedEventArgs e)
        {
            navigationPage.PopAllModalsAsync();
            _modalIndex = 0;
        }

        private static ContentPage CreateHomePage()
        {
            return new ContentPage
            {
                Header = "Home",
                Content = CreateContent("Root page", PagePalette[0])
            };
        }

        private static ContentPage CreatePage(int pageIndex, string header, string content)
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