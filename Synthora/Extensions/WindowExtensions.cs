using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Synthora.Extensions
{
    internal static class WindowExtensions 
    {
        public static T Find<T>(this TemplateAppliedEventArgs e, string elementName) where T : class
        {
            var element = e.NameScope.Find<T>(elementName);

            return element ?? throw new Exception($"\"{elementName}\" not found on Style");
        }

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