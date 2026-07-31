using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Synthora.Interop;

namespace Synthora.Attaches
{
    public class WindowAttach
    {
        public static readonly AttachedProperty<bool> UseDpiLayoutRoundingProperty =
            AvaloniaProperty.RegisterAttached<WindowAttach, Window, bool>("UseDpiLayoutRounding");

        public static readonly AttachedProperty<bool> SyncTitleBarThemeProperty =
            AvaloniaProperty.RegisterAttached<WindowAttach, Window, bool>("SyncTitleBarTheme");

        private static bool IsDarkMode => Application.Current?.ActualThemeVariant == ThemeVariant.Dark;

        static WindowAttach()
        {
            UseDpiLayoutRoundingProperty.Changed.AddClassHandler<Window, bool>((s, e) => OnUseDpiLayoutRoundingChanged(e));
            SyncTitleBarThemeProperty.Changed.AddClassHandler<Window, bool>((s, e) => OnSyncTitleBarThemeChanged(e));
        }

        public static bool GetUseDpiLayoutRounding(Window obj) => obj.GetValue(UseDpiLayoutRoundingProperty);
        public static void SetUseDpiLayoutRounding(Window obj, bool value) => obj.SetValue(UseDpiLayoutRoundingProperty, value);

        public static bool GetSyncTitleBarTheme(Window obj) => obj.GetValue(SyncTitleBarThemeProperty);
        public static void SetSyncTitleBarTheme(Window obj, bool value) => obj.SetValue(SyncTitleBarThemeProperty, value);

        private static void OnUseDpiLayoutRoundingChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not Window window)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                window.ScalingChanged -= Window_ScalingChanged;
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

        private static void UpdateUseLayoutRounding(Window window) => window.SetCurrentValue(Layoutable.UseLayoutRoundingProperty, ((int)(window.RenderScaling * 100)) % 2 != 0);

        private static void OnSyncTitleBarThemeChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not Window window || !Win32Interop.IsWindows10)
            {
                return;
            }

            if (e.NewValue.Value)
            {
                window.ActualThemeVariantChanged -= Window_ActualThemeVariantChanged;
                window.ActualThemeVariantChanged += Window_ActualThemeVariantChanged;
                SyncTitleBarTheme(window, IsDarkMode);
            }
            else
            {
                window.ActualThemeVariantChanged -= Window_ActualThemeVariantChanged;
                SyncTitleBarTheme(window, false);
            }
        }

        private static void Window_ActualThemeVariantChanged(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                SyncTitleBarTheme(window, IsDarkMode);
            }
        }

        private static void SyncTitleBarTheme(Window window, bool isDarkMode)
        {
            if (!window.IsExtendedIntoWindowDecorations && window.TryGetPlatformHandle() is { Handle: var handle })
            {
                Win32Interop.SetWindowTitleBarDarkMode(handle, isDarkMode);
            }
        }
    }
}