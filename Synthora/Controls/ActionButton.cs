using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Synthora.Controls
{
    internal class ActionButton : Button
    {
        public ActionButton()
        {
            AddHandler(PointerPressedEvent, InputElement_PointerPressed, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(PointerReleasedEvent, InputElement_PointerReleased, RoutingStrategies.Tunnel, handledEventsToo: true);
        }

        private void InputElement_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.Properties.IsLeftButtonPressed && ClickMode == ClickMode.Press)
            {
                OnPointerPressed(e);
                e.Handled = true;
            }
        }

        private void InputElement_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == MouseButton.Left && ClickMode == ClickMode.Release)
            {
                OnPointerReleased(e);
                e.Handled = true;
            }
        }
    }
}