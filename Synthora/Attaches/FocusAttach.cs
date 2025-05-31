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

        public static readonly AttachedProperty<DispatcherPriority> FocusPriorityProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, DispatcherPriority>("FocusPriority", DispatcherPriority.Render);

        static FocusAttach()
        {
            IsFocusedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsFocusedChanged(e));
        }

        public static bool GetIsFocused(InputElement obj) => obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(InputElement obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        public static DispatcherPriority GetFocusPriority(InputElement obj) => obj.GetValue(FocusPriorityProperty);
        public static void SetFocusPriority(InputElement obj, DispatcherPriority value) => obj.SetValue(FocusPriorityProperty, value);

        private static void OnIsFocusedChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not InputElement inputElement)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                var dispatcherPriority = GetFocusPriority(inputElement);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    inputElement.Focus();
                    if (inputElement is TextBox textBox)
                    {
                        textBox.SelectAll();
                    }
                }, dispatcherPriority);
            }
        }
    }
}