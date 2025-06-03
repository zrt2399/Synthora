using System;
using Avalonia.Input;

namespace Synthora.Utils
{
    /// <summary>
    /// Provides common keyboard shortcuts (key gestures) for application commands,
    /// using the Control key on Windows/Linux and the Command (Meta) key on macOS.
    /// </summary>
    public class GestureUtils
    {
        static GestureUtils()
        {
            CommandModifier = OperatingSystem.IsMacOS() ? KeyModifiers.Meta : KeyModifiers.Control;
            SaveGesture = new KeyGesture(Key.S, CommandModifier);
            SelectAllGesture = new KeyGesture(Key.A, CommandModifier);
            ZoomInGesture = new KeyGesture(Key.Add, CommandModifier);
            ZoomOutGesture = new KeyGesture(Key.Subtract, CommandModifier);
            ZoomInOemGesture = new KeyGesture(Key.OemPlus, CommandModifier);
            ZoomOutOemGesture = new KeyGesture(Key.OemMinus, CommandModifier);
        }

        /// <summary>
        /// Gets the platform-appropriate modifier key for commands:
        /// Control on Windows/Linux; Command (Meta) on macOS.
        /// </summary>
        public static KeyModifiers CommandModifier { get; }

        /// <summary>
        /// Gets the key gesture for the Save command (Ctrl+S or ⌘+S).
        /// </summary>
        public static KeyGesture SaveGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Select All command (Ctrl+A or ⌘+A).
        /// </summary>
        public static KeyGesture SelectAllGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom In using the Add key (Ctrl+Add or ⌘+Add).
        /// </summary>
        public static KeyGesture ZoomInGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom Out using the Subtract key (Ctrl+Subtract or ⌘+Subtract).
        /// </summary>
        public static KeyGesture ZoomOutGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom In using the OEM Plus key (Ctrl+'+' or ⌘+'+').
        /// </summary>
        public static KeyGesture ZoomInOemGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom Out using the OEM Minus key (Ctrl+'-' or ⌘+'-').
        /// </summary>
        public static KeyGesture ZoomOutOemGesture { get; }
    }
}