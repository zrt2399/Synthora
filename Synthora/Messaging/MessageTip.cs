using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Synthora.Controls;

namespace Synthora.Messaging
{
    /// <summary>
    /// Provides static methods to display transient message tips with various icon types.
    /// </summary>
    public static class MessageTip
    {
        private const int Delay = 2000;

        /// <summary>
        /// Global override for the display duration of all message tips (in milliseconds).
        /// When set to a value greater than 0, this will override the default <see cref="Delay"/>.
        /// </summary>
        public static int GlobalDelay { get; set; } = 0;

        /// <summary>
        /// Horizontal offset for positioning the message tip relative to the target element.
        /// </summary>
        public static double HorizontalOffset { get; set; } = 0;

        /// <summary>
        /// Vertical offset for positioning the message tip relative to the target element.
        /// </summary>
        public static double VerticalOffset { get; set; } = 12;

        /// <summary>
        /// Placement mode for the message tip (e.g., relative to a pointer or top).
        /// </summary>
        public static PlacementMode Placement { get; set; } = PlacementMode.Pointer;

        /// <summary>
        /// The element to which the message tip will be attached.
        /// If not explicitly set, defaults at runtime to:
        /// • the <see cref="IClassicDesktopStyleApplicationLifetime.MainWindow"/> for desktop apps, or  
        /// • the <see cref="ISingleViewApplicationLifetime.MainView"/> for single‐view apps.
        /// </summary>
        public static Control? PlacementTarget { get; set; }

        /// <summary>
        /// Displays a transient informational message tip with the default icon.
        /// </summary>
        public static void Show(string message, int delay = Delay) => Show(message, IconType.Information, delay);

        /// <summary>
        /// Displays a transient message tip with a question icon.
        /// </summary>
        public static void ShowQuestion(string message, int delay = Delay) => Show(message, IconType.Question, delay);

        /// <summary>
        /// Displays a transient message tip with a success icon.
        /// </summary>
        public static void ShowSuccess(string message, int delay = Delay) => Show(message, IconType.Success, delay);

        /// <summary>
        /// Displays a transient message tip with a warning icon.
        /// </summary>
        public static void ShowWarning(string message, int delay = Delay) => Show(message, IconType.Warning, delay);

        /// <summary>
        /// Displays a transient message tip with an error icon.
        /// </summary>
        public static void ShowError(string message, int delay = Delay) => Show(message, IconType.Error, delay);

        /// <summary>
        /// Displays a transient message tip with a specified icon type.
        /// </summary>
        public static async void Show(string message, IconType iconType, int delay = Delay)
        {
            if (Application.Current is not Application application)
            {
                return;
            }
            if (application.CheckAccess())
            {
                if (PlacementTarget == null)
                {
                    if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
                    {
                        PlacementTarget = classicDesktopStyleApplicationLifetime.MainWindow;
                    }
                    else if (application.ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
                    {
                        PlacementTarget = singleViewApplicationLifetime.MainView;
                    }
                    if (PlacementTarget == null)
                    {
                        return;
                    }
                }

                if (GlobalDelay > 0)
                {
                    delay = GlobalDelay;
                }

                var borderBrush = iconType switch
                {
                    IconType.Information => StatusIcon.InformationBackground,
                    IconType.Question => StatusIcon.QuestionBackground,
                    IconType.Success => StatusIcon.SuccessBackground,
                    IconType.Warning => StatusIcon.WarningBackground,
                    _ => StatusIcon.ErrorBackground
                };

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                TextBlock textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Text = message;

                if (!string.IsNullOrEmpty(textBlock.Text))
                {
                    textBlock.Margin = new Thickness(4, 0, 0, 0);
                }

                if (application.TryGetResource("FontSizeNormal", application.ActualThemeVariant, out var size) && size is double fontSize)
                {
                    textBlock.FontSize = fontSize;
                }

                textBlock.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(new Viewbox()
                {
                    Width = 16,
                    Height = 16,
                    Child = new StatusIcon()
                    {
                        IconType = iconType
                    }
                });
                grid.Children.Add(textBlock);

                Border border = new Border();
                border.Margin = new Thickness(4);
                border.Padding = new Thickness(4);
                if (application.TryGetResource("ThemeBorderCornerRadius", application.ActualThemeVariant, out var radius) && radius is CornerRadius cornerRadius)
                {
                    border.CornerRadius = cornerRadius;
                }
                border.Child = grid;
                border.Background = SolidColorBrush.Parse("#FAFAFA");
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = borderBrush;
                border.BoxShadow = new BoxShadows(new BoxShadow()
                {
                    Blur = 6,
                    Color = borderBrush.Color
                });

                Popup popup = new Popup();
                popup.Opened += Popup_Opened;
                popup.Child = border;
                popup.Placement = Placement;
                if (HorizontalOffset != 0)
                {
                    popup.HorizontalOffset = HorizontalOffset;
                }
                if (VerticalOffset != 0)
                {
                    popup.VerticalOffset = VerticalOffset;
                }
                popup.PlacementTarget = PlacementTarget;
                popup.IsOpen = true;
                await Task.Delay(delay);
                popup.IsOpen = false;
                popup.Opened -= Popup_Opened;
            }
            else
            {
                Dispatcher.UIThread.Invoke(() => Show(message, iconType, delay));
            }
        }

        private static void Popup_Opened(object? sender, EventArgs e)
        {
            if (sender is Popup popup && popup.Host is PopupRoot popupRoot)
            {
                popupRoot.Background = null;
                popupRoot.TransparencyBackgroundFallback = Brushes.Transparent;
                popupRoot.TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
            }
        }
    }
}