using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace Synthora.Attaches
{
    public class FocusAttach
    {
        public static readonly AttachedProperty<bool> IsFocusedProperty = 
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, bool>("IsFocused");

        static FocusAttach()
        {
            IsFocusedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsFocusedChanged(e));
        }

        public static bool GetIsFocused(InputElement obj) => obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(InputElement obj, bool value) => obj.SetValue(IsFocusedProperty, value); 

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