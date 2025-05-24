using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Synthora.Attaches
{
    public class FocusAdornerAttach
    {
        static FocusAdornerAttach()
        {
            UseCorrectFocusAdornerProperty.Changed.AddClassHandler<Control, bool>((s, e) => OnFocusAdornerChanged(e));
        }

        private static readonly ConditionalWeakTable<TemplatedControl, Control?> ControlToFocusAdornerMap = [];

        public static readonly AttachedProperty<bool> UseCorrectFocusAdornerProperty =
            AvaloniaProperty.RegisterAttached<TextBoxAttach, TemplatedControl, bool>("UseCorrectFocusAdorner");

        public static bool GetUseCorrectFocusAdorner(TemplatedControl textBox)
        {
            return textBox.GetValue(UseCorrectFocusAdornerProperty);
        }

        public static void SetUseCorrectFocusAdorner(TemplatedControl textBox, bool value)
        {
            textBox.SetValue(UseCorrectFocusAdornerProperty, value);
        }

        private static void OnFocusAdornerChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is TemplatedControl control)
            {
                if (e.OldValue.Value)
                {
                    control.RemoveHandler(InputElement.GotFocusEvent, OnGotFocus);
                    control.RemoveHandler(InputElement.LostFocusEvent, OnLostFocus);
                    ControlToFocusAdornerMap.Remove(control);
                }

                if (e.NewValue.Value)
                {
                    control.AddHandler(InputElement.GotFocusEvent, OnGotFocus, handledEventsToo: true);
                    control.AddHandler(InputElement.LostFocusEvent, OnLostFocus, handledEventsToo: true);
                }
            }
        }

        private static void OnGotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is TemplatedControl control && control.IsFocused && (e.NavigationMethod is NavigationMethod.Tab or NavigationMethod.Directional))
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(control);
                var focusTarget = control.GetTemplateChildren().FirstOrDefault(x => x.Name == "FocusTarget");
                if (adornerLayer != null && focusTarget != null)
                {
                    ControlToFocusAdornerMap.TryGetValue(control, out var focusAdorner);
                    if (focusAdorner == null)
                    {
                        var template = control.IsSet(Control.FocusAdornerProperty)
                            ? control.FocusAdorner
                            : adornerLayer.DefaultFocusAdorner;

                        focusAdorner = template?.Build();
                    }

                    if (focusAdorner != null)
                    {
                        if (adornerLayer.Children.Count > 0)
                        {
                            adornerLayer.Children.Remove(adornerLayer.Children.Last());
                        }
                        AdornerLayer.SetAdornedElement(focusAdorner, focusTarget);
                        adornerLayer.Children.Add(focusAdorner);
                        ControlToFocusAdornerMap.AddOrUpdate(control, focusAdorner);
                    }
                }
            }
        }

        private static void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is not TemplatedControl control)
            {
                return;
            }
            var adornerLayer = AdornerLayer.GetAdornerLayer(control);
            if (ControlToFocusAdornerMap.TryGetValue(control, out var focusAdorner) && focusAdorner != null && adornerLayer != null)
            {
                adornerLayer.Children.Remove(focusAdorner);
                ControlToFocusAdornerMap.Remove(control);
            }
        }
    }
}