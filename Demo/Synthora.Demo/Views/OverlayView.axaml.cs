using System.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Synthora.Controls;
using Synthora.Events;
using Synthora.Overlays;

namespace Synthora.Demo.Views
{
    public partial class OverlayView : UserControl
    {
        public OverlayView()
        {
            InitializeComponent();
        }

        private void CommandDialogHostDialogClosing(object? sender, DialogClosingEventArgs e)
        {
            if (sender is not DialogHost dialogHost)
            {
                return;
            }

            var confirmationCheckBox = dialogHost.GetVisualDescendants()
                .OfType<CheckBox>()
                .FirstOrDefault(x => x.Name == "ArchiveConfirmationCheckBox");

            if (confirmationCheckBox?.IsChecked == true)
            {
                return;
            }

            e.Cancel = true;
            MessageTip.ShowWarning("Confirm the archive action before closing.");
        }
    }
}