using System.ComponentModel;

namespace Synthora.Events;

public class TabClosingEventArgs(object? item) : CancelEventArgs
{
    public object? Item { get; } = item;
}