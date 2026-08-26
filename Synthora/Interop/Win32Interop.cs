using System;
using System.Runtime.InteropServices;

namespace Synthora.Interop
{
    internal enum DwmWindowAttribute : uint
    {
        NCRenderingEnabled = 1,
        NCRenderingPolicy,
        TransitionsForceDisabled,
        AllowNCPaint,
        CaptionButtonBounds,
        NonClientRtlLayout,
        ForceIconicRepresentation,
        Flip3DPolicy,
        ExtendedFrameBounds,
        HasIconicBitmap,
        DisallowPeek,
        ExcludedFromPeek,
        Cloak,
        Cloaked,
        FreezeRepresentation,
        PassiveUpdateMode,
        UseHostBackdropBrush,
        UseImmersiveDarkMode = 20,
        WindowCornerPreference = 33,
        BorderColor,
        CaptionColor,
        TextColor,
        VisibleFrameBorderThickness,
        SystemBackdropType,
        Last
    }

    internal static class Win32Interop
    {
        private const uint WM_NCACTIVATE = 0x0086;

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);

        public static bool IsWindows10
        {
            get
            {
                if (OperatingSystem.IsWindows())
                {
                    Version version = Environment.OSVersion.Version;
                    return version.Major == 10 && version.Build < 22000;
                }
                return false;
            }
        }

        public static bool SetWindowTitleBarDarkMode(IntPtr hWnd, bool enable)
        {
            if (hWnd != IntPtr.Zero && IsWindows10)
            {
                int darkMode = enable ? 1 : 0;
                int result = DwmSetWindowAttribute(hWnd, DwmWindowAttribute.UseImmersiveDarkMode, ref darkMode, sizeof(int));

                bool succeeded = result == 0;
                if (succeeded)
                {
                    RefreshTitleBar(hWnd);
                }

                return succeeded;
            }
            return false;
        }

        private static void RefreshTitleBar(IntPtr hWnd)
        {
            // 1. Get the actual active state of the current window
            bool isActive = GetActiveWindow() == hWnd;

            // 2. Determine the inverted (fake) state and the actual (final) state
            IntPtr falseState = isActive ? IntPtr.Zero : new IntPtr(1);
            IntPtr trueState = isActive ? new IntPtr(1) : IntPtr.Zero;

            // 3. Core workaround: Call DefWindowProc directly to force DWM to refresh 
            // without using SendMessage, which avoids triggering framework-level side effects.

            // Step 1: Apply the inverted state to force DWM to invalidate its cache
            DefWindowProc(hWnd, WM_NCACTIVATE, falseState, IntPtr.Zero);

            // Step 2: Immediately restore the actual state to ensure visual and physical focus remain correct
            DefWindowProc(hWnd, WM_NCACTIVATE, trueState, IntPtr.Zero);
        }
    }
}