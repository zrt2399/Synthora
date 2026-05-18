using Avalonia;
using Avalonia.Controls.Primitives;

namespace Synthora.Controls
{
    public class EmptyBox : TemplatedControl
    {
        public static readonly StyledProperty<double> IconWidthProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(IconWidth), 60d);

        public static readonly StyledProperty<double> IconHeightProperty =
            AvaloniaProperty.Register<EmptyBox, double>(nameof(IconHeight), 40d);

        public double IconWidth
        {
            get => GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public double IconHeight
        {
            get => GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }
    }
}