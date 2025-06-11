using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Synthora.Attaches
{
    public class FocusAttach
    {
        public static readonly AttachedProperty<bool> IsFocusedProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, bool>("IsFocused");

        public static readonly AttachedProperty<bool> FocusOnLoadedProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, Control, bool>("FocusOnLoaded");

        public static readonly AttachedProperty<DispatcherPriority> FocusPriorityProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, DispatcherPriority>("FocusPriority", DispatcherPriority.Render);

        public static readonly AttachedProperty<bool> UseCorrectFocusBehaviorProperty =
            AvaloniaProperty.RegisterAttached<FocusAttach, InputElement, bool>("UseCorrectFocusBehavior");

        static FocusAttach()
        {
            IsFocusedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsFocusedChanged(e));
            FocusOnLoadedProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnFocusOnLoadedChanged(e));
            UseCorrectFocusBehaviorProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnUseCorrectFocusChanged(e));
        }

        public static bool GetIsFocused(InputElement obj) => obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(InputElement obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        public static bool GetFocusOnLoaded(Control obj) => obj.GetValue(FocusOnLoadedProperty);
        public static void SetFocusOnLoaded(Control obj, bool value) => obj.SetValue(FocusOnLoadedProperty, value);

        public static DispatcherPriority GetFocusPriority(InputElement obj) => obj.GetValue(FocusPriorityProperty);
        public static void SetFocusPriority(InputElement obj, DispatcherPriority value) => obj.SetValue(FocusPriorityProperty, value);

        public static bool GetUseCorrectFocusBehavior(InputElement obj) => obj.GetValue(UseCorrectFocusBehaviorProperty);
        public static void SetUseCorrectFocusBehavior(InputElement obj, bool value) => obj.SetValue(UseCorrectFocusBehaviorProperty, value);

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
            if (sender is not Control control)
            {
                return;
            }

            var dispatcherPriority = GetFocusPriority(control);
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                control.Focus();
                if (sender is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }, dispatcherPriority);
        }

        private static void OnUseCorrectFocusChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not InputElement inputElement)
            {
                return;
            }

            inputElement.RemoveHandler(InputElement.GotFocusEvent, InputElement_GotFocus);
            if (e.NewValue.Value)
            {
                inputElement.AddHandler(InputElement.GotFocusEvent, InputElement_GotFocus, handledEventsToo: true);
            }
        }

        private static void InputElement_GotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is not InputElement inputElement)
            {
                return;
            }

            if (inputElement.GetVisualDescendants().FirstOrDefault(x => x is TextBox && x.Name == "PART_TextBox") is not TextBox textBox)
            {
                return;
            }

            if (Equals(e.Source, inputElement))
            {
                textBox.Focus(e.NavigationMethod, e.KeyModifiers);
                textBox.SelectAll();
                OnGotFocus(inputElement, e);
            }
        }

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(OnGotFocus))]
        private static extern void OnGotFocus(InputElement inputElement, GotFocusEventArgs e);
    }
}