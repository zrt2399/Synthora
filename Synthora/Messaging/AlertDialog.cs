using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Messaging
{
    /// <summary>
    /// Represents the possible results of a dialog interaction.
    /// </summary>
    public enum DialogResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No,
        Abort
    }

    /// <summary>
    /// Specifies which buttons to display in a dialog. Can be combined using a bitwise OR.
    /// </summary>
    [Flags]
    public enum DialogButton
    {
        None = 0,
        OK = 1,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,
        Abort = 1 << 4,
        OKCancel = OK | Cancel,
        YesNo = Yes | No,
        YesNoCancel = YesNo | Cancel
    }

    /// <summary>
    /// Defines the type of icon to display in a message or dialog.
    /// </summary>
    public enum IconType
    {
        Information,
        Question,
        Success,
        Warning,
        Error
    }

    public class AlertDialogDialogOptions
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DialogButton DialogButton { get; set; } = DialogButton.OK;
        public IconType IconType { get; set; }
        public bool ShowCloseButton { get; set; }
    }

    /// <summary>
    /// Provides methods to display alert dialogs with various buttons and icons.
    /// </summary>
    public static class AlertDialog
    {
        /// <summary>
        /// Displays an alert dialog asynchronously using the specified <paramref name="alertDialogDialogOptions"/>,
        /// targeting the <see cref="AlertDialogHost"/> instance identified by <paramref name="dialogIdentifier"/>.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string? dialogIdentifier, AlertDialogDialogOptions alertDialogDialogOptions)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                return await AlertDialogHost.ShowAsync(dialogIdentifier, alertDialogDialogOptions);
            }
            else
            {
                return await Dispatcher.UIThread.Invoke(() => ShowAsync(dialogIdentifier, alertDialogDialogOptions));
            }
        }

        /// <summary>
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance
        /// and the specified <paramref name="alertDialogDialogOptions"/>.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(AlertDialogDialogOptions alertDialogDialogOptions)
        {
            return await ShowAsync(null, alertDialogDialogOptions);
        }

        /// <summary>
        /// Displays an alert dialog asynchronously,
        /// using the <paramref name="dialogIdentifier"/> to locate the specific <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string? dialogIdentifier, string? message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(dialogIdentifier, new AlertDialogDialogOptions()
            {
                Message = message,
                Title = title,
                DialogButton = dialogButton,
                IconType = iconType
            });
        }

        /// <summary>
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string? message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(null, message, title, dialogButton, iconType);
        }
    }
}