using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using Synthora.Controls;
using Synthora.Resources;

namespace Synthora.Overlays
{
    /// <summary>
    /// Provides static methods to display transient message tips with various icon types.
    /// </summary>
    public static class MessageTip
    {
        private const int DefaultDelay = 2000;
        private static readonly IImmutableSolidColorBrush NoneBorderBrush = new ImmutableSolidColorBrush(Color.FromArgb(52, 0, 0, 0));
        private static readonly IImmutableSolidColorBrush NoneBackground = new ImmutableSolidColorBrush(Color.FromRgb(250, 250, 250));

        /// <summary>
        /// Display duration in milliseconds for all message tips.
        /// When set to a value greater than 0, this will override the default <see cref="DefaultDelay"/>.
        /// </summary>
        public static int Delay { get; set; } = 0;

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
        /// Displays a transient message tip with a question icon.
        /// </summary>
        public static void ShowQuestion(string message, int delay = DefaultDelay) => Show(message, IconType.Question, delay);

        /// <summary>
        /// Displays a transient message tip with a success icon.
        /// </summary>
        public static void ShowSuccess(string message, int delay = DefaultDelay) => Show(message, IconType.Success, delay);

        /// <summary>
        /// Displays a transient message tip with a warning icon.
        /// </summary>
        public static void ShowWarning(string message, int delay = DefaultDelay) => Show(message, IconType.Warning, delay);

        /// <summary>
        /// Displays a transient message tip with an error icon.
        /// </summary>
        public static void ShowError(string message, int delay = DefaultDelay) => Show(message, IconType.Error, delay);

        /// <summary>
        /// Displays a transient message tip with a specified icon type.
        /// </summary>
        public static async void Show(string message, IconType iconType = IconType.Information, int delay = DefaultDelay)
        {
            if (Application.Current is not { } application)
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

                if (Delay > 0)
                {
                    delay = Delay;
                }

                var borderBrush = iconType switch
                {
                    IconType.Information => SynthoraBrushes.InformationBrush,
                    IconType.Question => SynthoraBrushes.QuestionBrush,
                    IconType.Success => SynthoraBrushes.SuccessBrush,
                    IconType.Warning => SynthoraBrushes.WarningBrush,
                    IconType.Error => SynthoraBrushes.ErrorBrush,
                    _ => NoneBorderBrush
                };

                int padding = SynthoraTheme.GetCurrentDensity() switch
                {
                    ThemeDensity.Compact => 4,
                    ThemeDensity.Spacious => 6,
                    _ => 5,
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
                    textBlock.Margin = iconType == IconType.None ? new Thickness(2, 0) : new Thickness(padding, 0, 0, 0);
                }

                if (application.TryGetResource("FontSizeNormal", application.ActualThemeVariant, out var size) && size is double fontSize)
                {
                    textBlock.FontSize = fontSize;
                }

                textBlock.SetValue(Grid.ColumnProperty, 1);
                if (iconType != IconType.None)
                {
                    grid.Children.Add(new Viewbox()
                    {
                        Width = 16,
                        Height = 16,
                        Child = new StatusIcon()
                        {
                            IconType = iconType
                        }
                    });
                }

                grid.Children.Add(textBlock);

                Border border = new Border();
                if (application.TryGetResource("ThemeBorderCornerRadiusNormal", application.ActualThemeVariant, out var radius) && radius is CornerRadius cornerRadius)
                {
                    border.CornerRadius = cornerRadius;
                }

                border.Background = iconType switch
                {
                    IconType.Information => SynthoraBrushes.InformationTipBackgroundBrush,
                    IconType.Question => SynthoraBrushes.QuestionTipBackgroundBrush,
                    IconType.Success => SynthoraBrushes.SuccessTipBackgroundBrush,
                    IconType.Warning => SynthoraBrushes.WarningTipBackgroundBrush,
                    IconType.Error => SynthoraBrushes.ErrorTipBackgroundBrush,
                    _ => NoneBackground
                };

                border.Child = grid;
                border.Margin = new Thickness(4);
                border.Padding = new Thickness(padding);
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
                try
                {
                    popup.IsOpen = true;
                    await Task.Delay(delay);
                }
                finally
                {
                    popup.IsOpen = false;
                    popup.Opened -= Popup_Opened;
                }
            }
            else
            {
                Dispatcher.UIThread.Invoke(() => Show(message, iconType, delay));
            }
        }

        private static void Popup_Opened(object? sender, EventArgs e)
        {
            //if (sender is Popup popup && popup.Host is PopupRoot popupRoot)
            //{
            //    popupRoot.Background = null;
            //    popupRoot.TransparencyBackgroundFallback = Brushes.Transparent;
            //    popupRoot.TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
            //}

            if (sender is Popup popup && popup.Child != null) //Avalonia 12
            {
                // 获取 PopupRoot 的现代方法
                var topLevel = TopLevel.GetTopLevel(popup.Child);

                if (topLevel != null)
                {
                    topLevel.Background = null;
                    topLevel.TransparencyBackgroundFallback = Brushes.Transparent;
                    topLevel.TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
                }
            }
        }
    }
}