using Avalonia.Controls;

namespace Synthora.Demo.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainView _mainView = new();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Content = _mainView;
            };
        }
    }
}