﻿using System;
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
    /// Represents the possible results of a dialog interaction.
    /// </summary>
    public enum DialogResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No,
        Abort
    }

    /// <summary>
    /// Specifies which buttons to display in a dialog. Can be combined using a bitwise OR.
    /// </summary>
    [Flags]
    public enum DialogButton
    {
        None = 0,
        OK = 1,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,
        Abort = 1 << 4,
        OKCancel = OK | Cancel,
        YesNo = Yes | No,
        YesNoCancel = YesNo | Cancel
    }

    /// <summary>
    /// Defines the type of icon to display in a message or dialog.
    /// </summary>
    public enum IconType
    {
        Information,
        Question,
        Success,
        Warning,
        Error
    }

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
        /// Corner radius used for rounding the corners of the message tip.
        /// </summary>
        public static CornerRadius TipCornerRadius { get; set; } = new CornerRadius(4);

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
        public static void Show(string message, IconType iconType, int delay = Delay)
        {
            if (Application.Current is not Application application)
            {
                return;
            }
            if (application.CheckAccess())
            {
                Control? control = null;
                if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
                {
                    control = classicDesktopStyleApplicationLifetime.MainWindow;
                }
                else if (application.ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
                {
                    control = singleViewApplicationLifetime.MainView;
                }
                if (control == null)
                {
                    return;
                }

                Dispatcher.UIThread.Invoke(async () =>
                {
                    if (GlobalDelay > 0)
                    {
                        delay = GlobalDelay;
                    }

                    var borderBrush = iconType switch
                    {
                        IconType.Information => SolidColorBrush.Parse("#8C8C8C"),
                        IconType.Question => SolidColorBrush.Parse("#5A8CF0"),
                        IconType.Success => SolidColorBrush.Parse("#6EBE28"),
                        IconType.Warning => SolidColorBrush.Parse("#DC9B28"),
                        _ => SolidColorBrush.Parse("#E65050")
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

                    textBlock.FontSize = application.Resources["FontSizeNormal"] as double? ?? 12;
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
                    border.CornerRadius = TipCornerRadius;
                    border.Child = grid;
                    border.Background = SolidColorBrush.Parse("#FAFAFA");
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = borderBrush;
                    border.BoxShadow = new BoxShadows(new BoxShadow()
                    {
                        Blur = 8,
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
                    popup.PlacementTarget = control;
                    popup.IsOpen = true;
                    await Task.Delay(delay);
                    popup.IsOpen = false;
                    popup.Opened -= Popup_Opened;
                });
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