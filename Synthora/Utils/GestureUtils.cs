using System;
using Avalonia.Controls;
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
            SaveAsGesture = new KeyGesture(Key.S, CommandModifier | KeyModifiers.Shift);
            SelectAllGesture = new KeyGesture(Key.A, CommandModifier);
            ZoomInGesture = new KeyGesture(Key.Add, CommandModifier);
            ZoomOutGesture = new KeyGesture(Key.Subtract, CommandModifier);
            ZoomActualSizeGesture = new KeyGesture(Key.D0, CommandModifier);
            ZoomActualSizeNumPadGesture = new KeyGesture(Key.NumPad0, CommandModifier);
            ZoomInOemGesture = new KeyGesture(Key.OemPlus, CommandModifier);
            ZoomOutOemGesture = new KeyGesture(Key.OemMinus, CommandModifier);
            UndoGesture = new KeyGesture(Key.Z, CommandModifier);
            RedoGesture = new KeyGesture(Key.Y, CommandModifier);
            CutGesture = TextBox.CutGesture ?? new KeyGesture(Key.X, CommandModifier);
            CopyGesture = TextBox.CopyGesture ?? new KeyGesture(Key.C, CommandModifier);
            PasteGesture = TextBox.PasteGesture ?? new KeyGesture(Key.V, CommandModifier);
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
        /// Gets the key gesture for the Save As command (Ctrl+Shift+S or ⌘+Shift+S).
        /// </summary>
        public static KeyGesture SaveAsGesture { get; }

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
        /// Gets the key gesture for Actual Size zoom (Ctrl+0 or ⌘+0).
        /// </summary>
        public static KeyGesture ZoomActualSizeGesture { get; }

        /// <summary>
        /// Gets the key gesture for Actual Size zoom using the numpad 0 key (Ctrl+NumPad0 or ⌘+NumPad0).
        /// </summary>
        public static KeyGesture ZoomActualSizeNumPadGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom In using the OEM Plus key (Ctrl+'+' or ⌘+'+').
        /// </summary>
        public static KeyGesture ZoomInOemGesture { get; }

        /// <summary>
        /// Gets the key gesture for Zoom Out using the OEM Minus key (Ctrl+'-' or ⌘+'-').
        /// </summary>
        public static KeyGesture ZoomOutOemGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Undo command (Ctrl+Z or ⌘+Z).
        /// </summary>
        public static KeyGesture UndoGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Redo command (Ctrl+Y or ⌘+Y).
        /// </summary>
        public static KeyGesture RedoGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Cut command (Ctrl+X or ⌘+X).
        /// </summary>
        public static KeyGesture CutGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Copy command (Ctrl+C or ⌘+C).
        /// </summary>
        public static KeyGesture CopyGesture { get; }

        /// <summary>
        /// Gets the key gesture for the Paste command (Ctrl+V or ⌘+V).
        /// </summary>
        public static KeyGesture PasteGesture { get; }
    }
}