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

        static ScrollViewerAttach()
        {
            OrientationProperty.Changed.AddClassHandler<AvaloniaObject, Orientation>((s, e) => OnOrientationChanged(e));
        }

        public static Orientation GetOrientation(AvaloniaObject obj) => obj.GetValue(OrientationProperty);
        public static void SetOrientation(AvaloniaObject obj, Orientation value) => obj.SetValue(OrientationProperty, value);

        private static void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> e)
        {
            if (e.Sender is not ScrollViewer scrollViewer)
            {
                return;
            }
            if (e.NewValue.Value == Orientation.Horizontal)
            {
                scrollViewer.AddHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanged, handledEventsToo: true);
            }
            else
            {
                scrollViewer.RemoveHandler(InputElement.PointerWheelChangedEvent, ScrollViewerPointerWheelChanged);
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
    }
}