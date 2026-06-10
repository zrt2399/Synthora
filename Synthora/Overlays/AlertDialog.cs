using System;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Overlays
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
    /// Provides the options used to configure and display an alert dialog.
    /// </summary>
    public class AlertDialogOptions
    {
        public string? Title { get; set; }
        public string Message { get; set; } = string.Empty;
        public DialogButton DialogButton { get; set; } = DialogButton.OK;
        public IconType IconType { get; set; }
        public bool ShowCloseButton { get; set; }
        public bool IsFooterStretched { get; set; }
        public double MaxWidth { get; set; } = 800;
        public double MaxHeight { get; set; } = double.NaN;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
    }

    /// <summary>
    /// Provides methods to display alert dialogs with various buttons and icons.
    /// </summary>
    public static class AlertDialog
    {
        /// <summary>
        /// Returns whether the dialog with the given identifier is currently open.
        /// </summary>
        public static bool IsDialogOpen(string? dialogIdentifier = null) => AlertDialogHost.GetInstance(dialogIdentifier).IsOpen;

        /// <summary>
        /// Displays an alert dialog asynchronously using the specified <paramref name="alertDialogOptions"/>,
        /// targeting the <see cref="AlertDialogHost"/> instance identified by <paramref name="dialogIdentifier"/>.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string? dialogIdentifier, AlertDialogOptions alertDialogOptions)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                return await AlertDialogHost.GetInstance(dialogIdentifier).Show(alertDialogOptions);
            }
            else
            {
                return await Dispatcher.UIThread.Invoke(() => ShowAsync(dialogIdentifier, alertDialogOptions));
            }
        }

        /// <summary>
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance
        /// and the specified <paramref name="alertDialogOptions"/>.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(AlertDialogOptions alertDialogOptions)
        {
            return await ShowAsync(null, alertDialogOptions);
        }

        /// <summary>
        /// Displays an alert dialog asynchronously,
        /// using the <paramref name="dialogIdentifier"/> to locate the specific <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static async Task<DialogResult> ShowAsync(string? dialogIdentifier, string message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(dialogIdentifier, new AlertDialogOptions()
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
        public static async Task<DialogResult> ShowAsync(string message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return await ShowAsync(null, message, title, dialogButton, iconType);
        }

        /// <summary>
        /// Closes the most recently opened dialog for the specified host identifier.
        /// </summary>
        public static void CloseDialog(string? dialogIdentifier, DialogResult dialogResult)
        {
            var host = AlertDialogHost.GetInstance(dialogIdentifier);
            if (host.Dialogs.Count > 0)
            {
                host.Close(host.Dialogs[^1], dialogResult);
            }
        }

        /// <summary>
        /// Closes the most recently opened dialog for the default host instance.
        /// </summary>
        public static void CloseDialog(DialogResult dialogResult)
        {
            CloseDialog(null, dialogResult);
        }
    }
}