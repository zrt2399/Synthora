using System;
using Avalonia.Controls;

namespace Synthora.Events;
 
public class CloseLastTabEventArgs(Window? window) : EventArgs
{
    public Window? Window { get; } = window;
}