using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class WindowAttach
    {
        public static readonly AttachedProperty<bool> UseDpiLayoutRoundingProperty =
            AvaloniaProperty.RegisterAttached<WindowAttach, Window, bool>("UseDpiLayoutRounding");

        static WindowAttach()
        {
            UseDpiLayoutRoundingProperty.Changed.AddClassHandler<Window, bool>((s, e) => OnUseDpiLayoutRoundingChanged(e));
        }

        public static bool GetUseDpiLayoutRounding(Window obj) => obj.GetValue(UseDpiLayoutRoundingProperty);
        public static void SetUseDpiLayoutRounding(Window obj, bool value) => obj.SetValue(UseDpiLayoutRoundingProperty, value);
 
        private static void OnUseDpiLayoutRoundingChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not Window window)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                window.ScalingChanged += Window_ScalingChanged;
                UpdateUseLayoutRounding(window);
            }
            else
            {
                window.ScalingChanged -= Window_ScalingChanged;
                window.ClearValue(Layoutable.UseLayoutRoundingProperty);
            }
        }

        private static void Window_ScalingChanged(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                UpdateUseLayoutRounding(window);
            }
        }

        private static void UpdateUseLayoutRounding(Window window) => window.UseLayoutRounding = ((int)(window.RenderScaling * 100)) % 2 != 0;
    }
}