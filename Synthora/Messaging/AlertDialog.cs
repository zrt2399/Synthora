using System.Threading.Tasks;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Messaging
{
    /// <summary>
    /// Provides methods to display alert dialogs with various buttons and icons.
    /// </summary>
    public static class AlertDialog
    {
        /// <summary>
        /// Displays an alert dialog asynchronously,
        /// using the <paramref name="dialogIdentifier"/> to locate the specific <see cref="AlertDialogHost"/> instance.
        /// </summary>
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

        /// <summary>
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string message, string title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(null, message, title, dialogButton, iconType);
        }
    }
}