using Avalonia.Interactivity;

namespace Synthora.Events
{
    public class TabClosedEventArgs(object item) : RoutedEventArgs
    {
        public object Item { get; } = item;
    }
}