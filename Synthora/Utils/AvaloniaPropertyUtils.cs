using Avalonia;

namespace Synthora.Utils
{
    internal static class AvaloniaPropertyUtils
    {
        public static void SetValue<T>(this AvaloniaProperty<T> property, T value, params AvaloniaObject?[] objects)
        {
            foreach (var obj in objects)
            {
                obj?.SetValue(property, value);
            }
        }

        public static void SetCurrentValue<T>(this AvaloniaProperty<T> property, T value, params AvaloniaObject?[] objects)
        {
            foreach (var obj in objects)
            {
                obj?.SetCurrentValue(property, value);
            }
        }
    }
}