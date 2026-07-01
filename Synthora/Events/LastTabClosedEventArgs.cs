using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Synthora.Events
{
    public class LastTabClosedEventArgs(Window? window) : RoutedEventArgs
    {
        public Window? Window { get; } = window;
    }
}