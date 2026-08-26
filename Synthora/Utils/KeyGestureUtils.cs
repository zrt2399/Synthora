using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;

namespace Synthora.Utils
{
    /// <summary>
    /// Provides common keyboard shortcuts (key gestures) for application commands,
    /// using Avalonia's platform hotkey configuration with an OS-based fallback.
    /// </summary>
    public class KeyGestureUtils
    {
        private static PlatformHotkeyConfiguration? HotkeyConfiguration =>
            Application.Current?.PlatformSettings?.HotkeyConfiguration;

        static KeyGestureUtils()
        {
            CommandModifiers = HotkeyConfiguration?.CommandModifiers
                ?? (OperatingSystem.IsMacOS() ? KeyModifiers.Meta : KeyModifiers.Control);

            ZoomIn = new KeyGesture(Key.Add, CommandModifiers);
            ZoomOut = new KeyGesture(Key.Subtract, CommandModifiers);
            ZoomInOem = new KeyGesture(Key.OemPlus, CommandModifiers);
            ZoomOutOem = new KeyGesture(Key.OemMinus, CommandModifiers);
            ZoomActualSize = new KeyGesture(Key.D0, CommandModifiers);
            ZoomActualSizeNumPad = new KeyGesture(Key.NumPad0, CommandModifiers);
            Save = new KeyGesture(Key.S, CommandModifiers);
            SaveAs = new KeyGesture(Key.S, CommandModifiers | KeyModifiers.Shift);
            SelectAll = HotkeyConfiguration?.SelectAll.FirstOrDefault() ?? new KeyGesture(Key.A, CommandModifiers);
            Undo = HotkeyConfiguration?.Undo.FirstOrDefault() ?? new KeyGesture(Key.Z, CommandModifiers);
            Redo = HotkeyConfiguration?.Redo.FirstOrDefault() ?? new KeyGesture(Key.Y, CommandModifiers);
            Cut = TextBox.CutGesture ?? new KeyGesture(Key.X, CommandModifiers);
            Copy = TextBox.CopyGesture ?? new KeyGesture(Key.C, CommandModifiers);
            Paste = TextBox.PasteGesture ?? new KeyGesture(Key.V, CommandModifiers);
        }

        /// <summary>
        /// Gets the modifier keys configured by Avalonia for application commands.
        /// Falls back to Command (Meta) on macOS and Control on other platforms
        /// when the platform configuration is unavailable.
        /// </summary>
        public static KeyModifiers CommandModifiers { get; }

        /// <summary>
        /// Gets the key gesture for the Save command.
        /// </summary>
        public static KeyGesture Save { get; }

        /// <summary>
        /// Gets the key gesture for the Save As command.
        /// </summary>
        public static KeyGesture SaveAs { get; }

        /// <summary>
        /// Gets the key gesture for the Select All command.
        /// </summary>
        public static KeyGesture SelectAll { get; }

        /// <summary>
        /// Gets the key gesture for Zoom In using the Add key.
        /// </summary>
        public static KeyGesture ZoomIn { get; }

        /// <summary>
        /// Gets the key gesture for Zoom Out using the Subtract key.
        /// </summary>
        public static KeyGesture ZoomOut { get; }

        /// <summary>
        /// Gets the key gesture for Actual Size zoom.
        /// </summary>
        public static KeyGesture ZoomActualSize { get; }

        /// <summary>
        /// Gets the key gesture for Actual Size zoom using the numpad 0 key.
        /// </summary>
        public static KeyGesture ZoomActualSizeNumPad { get; }

        /// <summary>
        /// Gets the key gesture for Zoom In using the OEM Plus key.
        /// </summary>
        public static KeyGesture ZoomInOem { get; }

        /// <summary>
        /// Gets the key gesture for Zoom Out using the OEM Minus key.
        /// </summary>
        public static KeyGesture ZoomOutOem { get; }

        /// <summary>
        /// Gets the key gesture for the Undo command.
        /// </summary>
        public static KeyGesture Undo { get; }

        /// <summary>
        /// Gets the key gesture for the Redo command.
        /// </summary>
        public static KeyGesture Redo { get; }

        /// <summary>
        /// Gets the key gesture for the Cut command.
        /// </summary>
        public static KeyGesture Cut { get; }

        /// <summary>
        /// Gets the key gesture for the Copy command.
        /// </summary>
        public static KeyGesture Copy { get; }

        /// <summary>
        /// Gets the key gesture for the Paste command.
        /// </summary>
        public static KeyGesture Paste { get; }
    }
}