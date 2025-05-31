using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace Synthora.Attaches
{
    public class ScrollViewerAttach
    {
        public static readonly AttachedProperty<Orientation> OrientationProperty =
            AvaloniaProperty.RegisterAttached<ScrollViewerAttach, AvaloniaObject, Orientation>("Orientation", Orientation.Vertical, inherits: true);

        public static readonly AttachedProperty<bool> IsDisabledProperty =
            AvaloniaProperty.RegisterAttached<ScrollViewerAttach, InputElement, bool>("IsDisabled");

        static ScrollViewerAttach()
        {
            OrientationProperty.Changed.AddClassHandler<AvaloniaObject, Orientation>((s, e) => OnOrientationChanged(e));
            IsDisabledProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsDisabledChanged(e));
        }

        public static Orientation GetOrientation(AvaloniaObject obj) => obj.GetValue(OrientationProperty);
        public static void SetOrientation(AvaloniaObject obj, Orientation value) => obj.SetValue(OrientationProperty, value);

        public static bool GetIsDisabled(InputElement obj) => obj.GetValue(IsDisabledProperty);
        public static void SetIsDisabled(InputElement obj, bool value) => obj.SetValue(IsDisabledProperty, value);

        private static void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> e)
        {
            if (e.Sender is not ScrollViewer scrollViewer)
            {
                return;
            }
            
            scrollViewer.RemoveHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanged);
            if (e.NewValue.Value == Orientation.Horizontal)
            {
                scrollViewer.AddHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanged, handledEventsToo: true);
            }
        }

        private static void ScrollViewerPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            const int step = 50;

            if (sender is not ScrollViewer scrollViewer)
            {
                return;
            }

            scrollViewer.Offset = new Vector(
                scrollViewer.Offset.X - e.Delta.Y * step,
                scrollViewer.Offset.Y
            );

            e.Handled = true;
        }

        private static void OnIsDisabledChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not InputElement inputElement)
            {
                return;
            }

            inputElement.RemoveHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanging);
            if (e.NewValue.Value)
            {
                inputElement.AddHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanging, handledEventsToo: true);
            }
        }

        private static void ScrollViewerPointerWheelChanging(object? sender, PointerWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}