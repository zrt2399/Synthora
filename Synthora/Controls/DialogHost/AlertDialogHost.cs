using System.Threading.Tasks;
using Avalonia.Layout;
using Synthora.Overlays;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a single alert dialog instance currently managed by an <see cref="AlertDialogHost"/>.
    /// </summary>
    public class AlertDialogInstance : DialogHostInstance
    {
        public string? Title { get; }
        public string Message { get; }
        public DialogButton DialogButton { get; }
        public IconType IconType { get; }
        public bool ShowCloseButton { get; }
        public bool IsFooterStretched { get; }
        public double MaxWidth { get; }
        public double MaxHeight { get; }
        public override HorizontalAlignment HorizontalAlignment { get; }
        public override VerticalAlignment VerticalAlignment { get; }
        internal new TaskCompletionSource<DialogResult> Tcs { get; } = new TaskCompletionSource<DialogResult>();

        public AlertDialogInstance(AlertDialogHost host, AlertDialogOptions options) : base(host, options)
        {
            Title = options.Title;
            Message = options.Message;
            DialogButton = options.DialogButton;
            IconType = options.IconType;
            ShowCloseButton = options.ShowCloseButton || DialogButton == DialogButton.None;
            IsFooterStretched = options.IsFooterStretched;
            MaxWidth = options.MaxWidth;
            MaxHeight = options.MaxHeight;
            HorizontalAlignment = options.HorizontalAlignment;
            VerticalAlignment = options.VerticalAlignment;
        }

        public void OK() => Host.CloseDialog(this, DialogResult.OK);
        public void Cancel() => Host.CloseDialog(this, DialogResult.Cancel);
        public void Yes() => Host.CloseDialog(this, DialogResult.Yes);
        public void No() => Host.CloseDialog(this, DialogResult.No);
        public void Abort() => Host.CloseDialog(this, DialogResult.Abort);
        public void None() => Host.CloseDialog(this, DialogResult.None);

        internal override void SetResult(object? parameter)
        {
            var dialogResult = parameter is DialogResult result ? result : DialogResult.None;
            Tcs.TrySetResult(dialogResult);
        }
    }

    /// <summary>
    /// Hosts alert dialogs and manages their overlay state.
    /// </summary>
    public class AlertDialogHost : DialogHost
    {
        /// <summary>
        /// Finds a loaded dialog host by identifier.
        /// </summary>
        public static new AlertDialogHost GetInstance(string? dialogIdentifier) => GetInstance<AlertDialogHost>(dialogIdentifier);

        public Task<DialogResult> ShowDialog(AlertDialogOptions alertDialogOptions)
        {
            if (CheckAccess())
            {
                var dialogInstance = new AlertDialogInstance(this, alertDialogOptions);
                OpenDialog(dialogInstance);

                return dialogInstance.Tcs.Task;
            }
            return Dispatcher.Invoke(() => ShowDialog(alertDialogOptions));
        }
    }
}