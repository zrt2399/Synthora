using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Synthora.Controls
{
    internal class ActionButton : Button
    {
        public ActionButton()
        {
            AddHandler(PointerReleasedEvent, InputElement_PointerPressed, RoutingStrategies.Tunnel, handledEventsToo: true);
        }

        private void InputElement_PointerPressed(object? sender, PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == MouseButton.Left)
            {
                Command?.Execute(CommandParameter);
                e.Handled = true;
            }  
        }
    }
}