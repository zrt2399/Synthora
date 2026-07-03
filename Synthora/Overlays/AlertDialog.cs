using System;
using System.Threading.Tasks;
using Avalonia.Layout;
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
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance
        /// and the specified <paramref name="alertDialogOptions"/>.
        /// </summary>
        public static Task<DialogResult> Show(AlertDialogOptions alertDialogOptions, string? dialogIdentifier = null)
        {
            return AlertDialogHost.GetInstance(dialogIdentifier).ShowDialog(alertDialogOptions);
        }

        /// <summary>
        /// Displays an alert dialog asynchronously,
        /// using the <paramref name="dialogIdentifier"/> to locate the specific <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static Task<DialogResult> Show(string? dialogIdentifier, string message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return Show(new AlertDialogOptions()
            {
                Message = message,
                Title = title,
                DialogButton = dialogButton,
                IconType = iconType
            }, dialogIdentifier);
        }

        /// <summary>
        /// Displays an alert dialog asynchronously using the default <see cref="AlertDialogHost"/> instance.
        /// </summary>
        public static Task<DialogResult> Show(string message, string? title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            return Show(null, message, title, dialogButton, iconType);
        }

        /// <summary>
        /// Closes the most recently opened dialog for the specified host identifier.
        /// </summary>
        public static void Close(DialogResult dialogResult, string? dialogIdentifier = null, AlertDialogOptions? alertDialogOptions = null)
        {
            var host = AlertDialogHost.GetInstance(dialogIdentifier);
            if (host.Dialogs.Count > 0)
            {
                host.CloseDialog(dialogResult, alertDialogOptions);
            }
        }
    }
}