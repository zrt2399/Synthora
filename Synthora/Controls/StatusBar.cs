using Avalonia;
using Avalonia.Controls;

namespace Synthora.Controls
{
    public class StatusBar : ContentControl
    {
        public static readonly StyledProperty<bool> IsRunningProperty =
            AvaloniaProperty.Register<StatusBar, bool>(nameof(IsRunning));

        public bool IsRunning
        {
            get => GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }
    }
}