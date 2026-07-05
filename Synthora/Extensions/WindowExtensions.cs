using Avalonia;
using Avalonia.Controls;

namespace Synthora.Extensions
{
    internal static class WindowExtensions 
    { 
        public static void RestoreWindow(this Window? window)
        {
            if (window is null)
            {
                return;
            }

            if (!window.CanResize)
            {
                return;
            }

            if (window.WindowState == WindowState.FullScreen)
            {
                return;
            }

            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
 
        public static void DragWindow(this Window? window, double vectorX, double vectorY)
        {
            if (window is null)
            {
                return;
            }

            var pos = window.Position;

            window.Position = new PixelPoint(
                x: (int)(pos.X + vectorX),
                y: (int)(pos.Y + vectorY));
        }
    }
}