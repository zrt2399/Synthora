using System;
using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Synthora.Attaches
{
    public class CornerRadiusAttach
    {
        public static readonly AttachedProperty<bool> IsCircularProperty =
            AvaloniaProperty.RegisterAttached<CornerRadiusAttach, InputElement, bool>("IsCircular");

        private static readonly ConcurrentDictionary<InputElement, EventHandler<AvaloniaPropertyChangedEventArgs>> Handlers = [];

        static CornerRadiusAttach()
        {
            IsCircularProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsCircularChanged(e));
        }

        public static bool GetIsCircular(InputElement element) => element.GetValue(IsCircularProperty);
        public static void SetIsCircular(InputElement element, bool value) => element.SetValue(IsCircularProperty, value);

        private static void OnIsCircularChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not InputElement element)
            {
                return;
            }

            if (GetCornerRadiusProperty(element) is not { } property)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                if (Handlers.ContainsKey(element))
                {
                    return;
                }

                UpdateCornerRadius(element, property);

                EventHandler<AvaloniaPropertyChangedEventArgs> handler = (_, args) =>
                {
                    if (args.Property == Visual.BoundsProperty)
                    {
                        UpdateCornerRadius(element, property);
                    }
                };

                element.PropertyChanged += handler;
                Handlers[element] = handler;
            }
            else
            {
                if (Handlers.TryGetValue(element, out var handler))
                {
                    element.PropertyChanged -= handler;
                    Handlers.TryRemove(element, out _);
                }

                element.ClearValue(property);
            }
        }

        private static void UpdateCornerRadius(InputElement element, AvaloniaProperty<CornerRadius> property)
        {
            double radius = Math.Min(element.Bounds.Width, element.Bounds.Height) / 2;
            element.SetCurrentValue(property, new CornerRadius(radius));
        }

        private static AvaloniaProperty<CornerRadius>? GetCornerRadiusProperty(InputElement element)
        {
            if (element is Border)
            {
                return Border.CornerRadiusProperty;
            }
            else if (element is TemplatedControl)
            {
                return TemplatedControl.CornerRadiusProperty;
            }
            return null;
        }
    }
}