using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class StatusBar : ContentControl
    {
        public static readonly StyledProperty<bool> IsRunningProperty =
            AvaloniaProperty.Register<StatusBar, bool>(nameof(IsRunning));

        public static readonly StyledProperty<IBrush?> RunningBackgroundProperty =
            AvaloniaProperty.Register<StatusBar, IBrush?>(nameof(RunningBackground));

        public static readonly StyledProperty<IBrush?> RunningForegroundProperty =
            AvaloniaProperty.Register<StatusBar, IBrush?>(nameof(RunningForeground));

        public bool IsRunning
        {
            get => GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        public IBrush? RunningBackground
        {
            get => GetValue(RunningBackgroundProperty);
            set => SetValue(RunningBackgroundProperty, value);
        }
        
        public IBrush? RunningForeground
        {
            get => GetValue(RunningForegroundProperty);
            set => SetValue(RunningForegroundProperty, value);
        }
    }
}