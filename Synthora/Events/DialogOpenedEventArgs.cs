using System;
using Avalonia.Interactivity;
using Synthora.Controls;

namespace Synthora.Events
{
    public class DialogOpenedEventArgs(DialogHostInstance dialogHostInstance) : RoutedEventArgs
    {
        public DialogHostInstance DialogHostInstance { get; } = dialogHostInstance;

        public object? Content => DialogHostInstance.Content;
    }
}