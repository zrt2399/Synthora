using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Synthora.Demo.Views
{
    public partial class RefreshContainerView : UserControl
    {
        public RefreshContainerView()
        {
            InitializeComponent();
        }

        private async void RefreshContainerRefreshRequested(object? sender, RefreshRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();

            try
            {
                RefreshStatusText.Text = $"Feed refreshed {DateTime.Now:HH:mm:ss}";
                await Task.Delay(700);
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}