using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace Synthora.Attaches
{
    public class FocusAttach
    {
        static FocusAttach()
        {
            IsFocusedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsFocusedChanged(e));
        }

        public static readonly AttachedProperty<bool> IsFocusedProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, bool>("IsFocused");

        public static void SetIsFocused(InputElement obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(InputElement obj)
        {
            return obj.GetValue(IsFocusedProperty);
        }

        private static void OnIsFocusedChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not InputElement inputElement)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    inputElement.Focus();
                    if (inputElement is TextBox textBox)
                    {
                        textBox.SelectAll();
                    }
                }, DispatcherPriority.Render);
            }
        }
    }
}