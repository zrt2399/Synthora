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
    public enum DialogResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No,
        Abort
    }

    [Flags]
    public enum DialogButton
    {
        None = 0,
        OK = 1,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,
        Abort = 1 << 4
    }

    public enum IconType
    {
        Information,
        Question,
        Success,
        Warning,
        Error
    }

    public static class MessageTip
    {
        private const int Delay = 2000;

        public static int GlobalDelay { get; set; } = 0;
        public static double HorizontalOffset { get; set; } = 0;
        public static double VerticalOffset { get; set; } = 0;
        public static CornerRadius TipCornerRadius { get; set; } = new CornerRadius(4);

        public static void Show(string message, int delay = Delay) => Show(message, IconType.Information, delay);

        public static void ShowQuestion(string message, int delay = Delay) => Show(message, IconType.Question, delay);

        public static void ShowSuccess(string message, int delay = Delay) => Show(message, IconType.Success, delay);

        public static void ShowWarning(string message, int delay = Delay) => Show(message, IconType.Warning, delay);

        public static void ShowError(string message, int delay = Delay) => Show(message, IconType.Error, delay);

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
                    grid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
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
                    popup.Placement = PlacementMode.Pointer;
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