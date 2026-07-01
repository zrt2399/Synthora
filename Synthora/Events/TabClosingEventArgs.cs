using Avalonia.Interactivity;

namespace Synthora.Events
{
    public class TabClosingEventArgs(object? item) : CancelRoutedEventArgs
    {
        public object? Item { get; } = item;
    }
}