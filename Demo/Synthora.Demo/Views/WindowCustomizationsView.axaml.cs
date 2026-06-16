using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;

namespace Synthora.Demo.Views
{
    public partial class WindowCustomizationsView : UserControl
    {
        private IInsetsManager? _insetsManager;
        private bool _isRefreshingInsetsControls;

        public WindowCustomizationsView()
        {
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            InitializeInsetsManager();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            CleanupInsetsManager();
            base.OnDetachedFromVisualTree(e);
        }

        private void InitializeInsetsManager()
        {
            if (TopLevel.GetTopLevel(this)?.InsetsManager is not { } insetsManager)
            {
                return;
            }

            if (ReferenceEquals(_insetsManager, insetsManager))
            {
                return;
            }

            CleanupInsetsManager();

            _insetsManager = insetsManager;
            RefreshInsetsControls();

            insetsManager.SafeAreaChanged += InsetsManagerSafeAreaChanged;
            IsSystemBarVisibleCheckBox.IsCheckedChanged += InsetsCheckBoxPropertyChanged;
            DisplayEdgeToEdgeCheckBox.IsCheckedChanged += InsetsCheckBoxPropertyChanged;
        }

        private void CleanupInsetsManager()
        {
            if (_insetsManager is not null)
            {
                _insetsManager.SafeAreaChanged -= InsetsManagerSafeAreaChanged;
            }

            IsSystemBarVisibleCheckBox.IsCheckedChanged -= InsetsCheckBoxPropertyChanged;
            DisplayEdgeToEdgeCheckBox.IsCheckedChanged -= InsetsCheckBoxPropertyChanged;

            _insetsManager = null;
        }

        private void InsetsManagerSafeAreaChanged(object? sender, SafeAreaChangedArgs e)
        {
            RefreshInsetsControls();
        }

        private async void InsetsCheckBoxPropertyChanged(object? sender, RoutedEventArgs e)
        {
            if (_isRefreshingInsetsControls || _insetsManager is not { } insetsManager)
            {
                return;
            }

            if (ReferenceEquals(sender, DisplayEdgeToEdgeCheckBox))
            {
                insetsManager.DisplayEdgeToEdgePreference = DisplayEdgeToEdgeCheckBox.IsChecked == true;
            }
            else if (ReferenceEquals(sender, IsSystemBarVisibleCheckBox))
            {
                insetsManager.IsSystemBarVisible = IsSystemBarVisibleCheckBox.IsChecked == true;
            }
            else
            {
                return;
            }

            await Task.Delay(100);

            if (ReferenceEquals(_insetsManager, insetsManager))
            {
                RefreshInsetsControls();
            }
        }

        private void RefreshInsetsControls()
        {
            if (_insetsManager is not { } insetsManager)
            {
                return;
            }

            _isRefreshingInsetsControls = true;
            try
            {
                SafeAreaPadding.Text = $"Safe Area Padding: {insetsManager.SafeAreaPadding}";
                DisplayEdgeToEdgeCheckBox.IsChecked = insetsManager.DisplayEdgeToEdgePreference;
                IsSystemBarVisibleCheckBox.IsChecked = insetsManager.IsSystemBarVisible ?? true;
            }
            finally
            {
                _isRefreshingInsetsControls = false;
            }
        }
    }
}