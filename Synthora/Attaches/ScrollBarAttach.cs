using Avalonia;
using Avalonia.Input;

namespace Synthora.Attaches
{
    public class ScrollBarAttach
    {
        public static readonly AttachedProperty<bool> InnerOffsetEnabledProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarAttach, InputElement, bool>("InnerOffsetEnabled", defaultValue: true);

        public static bool GetInnerOffsetEnabled(InputElement obj) => obj.GetValue(InnerOffsetEnabledProperty);
        public static void SetInnerOffsetEnabled(InputElement obj, bool value) => obj.SetValue(InnerOffsetEnabledProperty, value);
    }
}