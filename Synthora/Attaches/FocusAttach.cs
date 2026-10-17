using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Synthora.Attaches
{
    public class FocusAttach
    {
        public static readonly AttachedProperty<bool> IsFocusedProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, bool>("IsFocused");

        public static readonly AttachedProperty<bool> FocusOnLoadedProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, Control, bool>("FocusOnLoaded");

        public static readonly AttachedProperty<DispatcherPriority> FocusPriorityProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, DispatcherPriority>("FocusPriority", DispatcherPriority.Loaded);

        static FocusAttach()
        {
            IsFocusedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsFocusedChanged(e));
            FocusOnLoadedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnFocusOnLoadedChanged(e));
        }

        public static bool GetIsFocused(InputElement obj) => obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(InputElement obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        public static bool GetFocusOnLoaded(Control obj) => obj.GetValue(FocusOnLoadedProperty);
        public static void SetFocusOnLoaded(Control obj, bool value) => obj.SetValue(FocusOnLoadedProperty, value);

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
                    FocusElement(inputElement);
                }, dispatcherPriority);
            }
        }

        private static void OnFocusOnLoadedChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not Control control)
            {
                return;
            }

            control.RemoveHandler(Control.LoadedEvent, Control_Loaded);
            if (e.NewValue.Value)
            {
                control.AddHandler(Control.LoadedEvent, Control_Loaded, handledEventsToo: true);
            }
        }

        private static void Control_Loaded(object? sender, RoutedEventArgs e)
        {
            if (sender is not InputElement inputElement)
            {
                return;
            }

            var dispatcherPriority = GetFocusPriority(inputElement);
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                FocusElement(inputElement);
            }, dispatcherPriority);
        }

        private static void FocusElement(InputElement inputElement)
        {
            if (!inputElement.IsVisible)
            {
                return;
            }
            if (inputElement is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
            }
            else if (inputElement is Panel panel && panel.Children.FirstOrDefault(x => x.IsVisible && x.Focusable) is InputElement element)
            {
                element.Focus();
            }
            else
            {
                inputElement.Focus();
            }
        }
    }
}