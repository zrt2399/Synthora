using System;

namespace Synthora.Events;

public class TabClosedEventArgs(object item) : EventArgs
{
    public object Item { get; } = item;
}