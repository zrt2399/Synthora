using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Messaging
{
    public static class AlertDialog
    {
        private static readonly List<bool> _instances = [];
        public static bool IsOpen => _instances.Count != 0;

        public static async Task<DialogResult> ShowAsPopupAsync(ContentControl owner, string message, string title, DialogButton dialogButton = DialogButton.OK, IconType iconType = IconType.Information)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                try
                {
                    _instances.Add(true);
                    var tcs = new TaskCompletionSource<DialogResult>(); 
                    var parentContent = owner.Content;
                    //owner.Content = null;

                    var alertDialogHost = new AlertDialogHost
                    {
                        ParentContent = parentContent,
                        Content = message,
                        Title = title,
                        DialogButton = dialogButton,
                        IconType = iconType 
                    };

                    alertDialogHost.DialogClosed += OnDialogClosed; 
                    owner.Content = alertDialogHost; 
                    var dialogResult = await tcs.Task;
                    alertDialogHost.DialogClosed -= OnDialogClosed;
                    return dialogResult;

                    void OnDialogClosed(object? sender, DialogClosedEventArgs e)
                    {
                        //owner.Content = null;
                        alertDialogHost.Content = null;
                        alertDialogHost.ParentContent = null;
                        owner.Content = parentContent;
                        tcs.TrySetResult(e.DialogResult);
                    }
                }
                finally
                {
                    _instances.Remove(true);
                } 
            }
            else
            {
                return await Dispatcher.UIThread.Invoke(() => ShowAsPopupAsync(owner, message, title, dialogButton, iconType));
            }
        }
    }
}