using System;
using Avalonia.Input;

namespace Synthora.Utils
{
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

        public static KeyModifiers CommandModifier { get; }
        public static KeyGesture SaveGesture { get; }
        public static KeyGesture SelectAllGesture { get; }
        public static KeyGesture ZoomInGesture { get; }
        public static KeyGesture ZoomOutGesture { get; }
        public static KeyGesture ZoomInOemGesture { get; }
        public static KeyGesture ZoomOutOemGesture { get; }
    }
}