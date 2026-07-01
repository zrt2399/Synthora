using Avalonia.Interactivity;
using Synthora.Controls;

namespace Synthora.Events
{
    public class DialogClosingEventArgs(DialogHostInstance dialogHostInstance, object? parameter) : CancelRoutedEventArgs
    {
        public DialogHostInstance DialogHostInstance { get; } = dialogHostInstance;

        public object? Content => DialogHostInstance.Content;

        public object? Parameter { get; } = parameter;
    }
}