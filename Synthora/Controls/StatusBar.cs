using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a status bar that can display a running state.
    /// </summary>
    public class StatusBar : ContentControl
    {
        /// <summary>
        /// Defines the <see cref="IsRunning"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsRunningProperty =
            AvaloniaProperty.Register<StatusBar, bool>(nameof(IsRunning));

        /// <summary>
        /// Defines the <see cref="RunningBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> RunningBackgroundProperty =
            AvaloniaProperty.Register<StatusBar, IBrush?>(nameof(RunningBackground));

        /// <summary>
        /// Defines the <see cref="RunningForeground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> RunningForegroundProperty =
            AvaloniaProperty.Register<StatusBar, IBrush?>(nameof(RunningForeground));

        /// <summary>
        /// Gets or sets whether the status bar is in the running state.
        /// </summary>
        public bool IsRunning
        {
            get => GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush used while the status bar is running.
        /// </summary>
        public IBrush? RunningBackground
        {
            get => GetValue(RunningBackgroundProperty);
            set => SetValue(RunningBackgroundProperty, value);
        }
        
        /// <summary>
        /// Gets or sets the foreground brush used while the status bar is running.
        /// </summary>
        public IBrush? RunningForeground
        {
            get => GetValue(RunningForegroundProperty);
            set => SetValue(RunningForegroundProperty, value);
        }
    }
}