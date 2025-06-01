using System.Threading.Tasks;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Messaging
{
    public static class AlertDialog
    {
        public static async Task<DialogResult> ShowAsync(string? dialogIdentifier, string message, string title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                return await AlertDialogHost.ShowAsync(dialogIdentifier, message, title, dialogButton, iconType);
            }
            else
            {
                return await Dispatcher.UIThread.Invoke(() => ShowAsync(dialogIdentifier, message, title, dialogButton, iconType));
            }
        }
 
        public static async Task<DialogResult> ShowAsync(string message, string title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(null, message, title, dialogButton, iconType);
        }
    }
}