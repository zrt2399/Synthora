using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Synthora.Utils;

namespace Synthora.Messaging
{
    public enum IconType
    {
        Info,
        OK,
        Warning,
        Error
    }

    public static class MessageTip
    {
        private const int Delay = 2000;

        public static int GlobalDelay { get; set; } = 0;

        public static CornerRadius TipCornerRadius { get; set; } = new CornerRadius(4);

        public static void Show(string message, int delay = Delay) => Show(message, IconType.Info, delay);

        public static void ShowOK(string message, int delay = Delay) => Show(message, IconType.OK, delay);

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
                        IconType.Info => SolidColorBrush.Parse("#8C8C8C"),
                        IconType.OK => SolidColorBrush.Parse("#6EBE28"),
                        IconType.Warning => SolidColorBrush.Parse("#DC9B28"),
                        _ => SolidColorBrush.Parse("#E65050")
                    };

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    TextBlock textBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, Text = message };

                    if (!string.IsNullOrEmpty(textBlock.Text))
                    {
                        textBlock.Margin = new Thickness(4, 0, 0, 0);
                    }

                    textBlock.FontSize = application.Resources["FontSizeNormal"] as double? ?? 12;
                    textBlock.SetValue(Grid.ColumnProperty, 1);
                    grid.Children.Add(new Viewbox() { Width = 20, Height = 20, Child = new TipIcon() { IconType = iconType, Margin = new Thickness(2) } });
                    grid.Children.Add(textBlock);

                    Border border = new Border();
                    border.Margin = new Thickness(4);
                    border.Padding = new Thickness(4);
                    border.CornerRadius = TipCornerRadius;
                    border.Child = grid;
                    border.Background = SolidColorBrush.Parse("#FAFAFA");
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = borderBrush;
                    border.BoxShadow = new BoxShadows(new BoxShadow() { Blur = 6, Color = borderBrush.Color });

                    Popup popup = new Popup();
                    popup.Opened += Popup_Opened;
                    popup.Child = border;
                    //if (control is Window window && window.Screens.Primary is Screen screen)
                    //{
                    //    grid.MaxWidth = screen.WorkingArea.Width;
                    //}
                    popup.Placement = PlacementMode.Pointer;
                    //popup.VerticalOffset = 16;
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

    public class TipIcon : Control
    {
        public static readonly StyledProperty<IconType> IconTypeProperty =
            AvaloniaProperty.Register<TipIcon, IconType>(nameof(IconType), IconType.Info);

        static TipIcon()
        {
            AffectsRender<TipIcon>(IconTypeProperty);
        }

        public TipIcon()
        {
            Height = 20;
            Width = 20;
        }

        private double ActualHeight => Bounds.Height;
        private double ActualWidth => Bounds.Width;

        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        public override void Render(DrawingContext drawingContext)
        {
            if (IconType == IconType.Info)
            {
                var background = new SolidColorBrush(Color.FromRgb(103, 148, 186));
                var pen = new Pen(background, 2);
                drawingContext.DrawEllipse(background, pen, new Point(ActualHeight / 2, ActualWidth / 2), ActualHeight / 2 - 1, ActualWidth / 2 - 1);

                var exclamationPen = new Pen(new SolidColorBrush(Color.FromRgb(245, 245, 245)), 2);
                drawingContext.DrawLine(exclamationPen, new Point(10, 4), new Point(10, 6));
                drawingContext.DrawLine(exclamationPen, new Point(10, 8), new Point(10, 16));
            }
            else if (IconType == IconType.OK)
            {
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(1.4, 10), false);
                    ctx.LineTo(new Point(8, 16));
                    ctx.LineTo(new Point(18.4, 2));
                }
                var background = new SolidColorBrush(Color.FromRgb(110, 190, 40));
                var pen = new Pen(background, 4);
                drawingContext.DrawGeometry(background, pen, geometry);
            }
            else if (IconType == IconType.Warning)
            {
                var points = new[] { new Point(10, 1), new Point(19.2, 18.6), new Point(0.8, 18.6) };
                var background = new SolidColorBrush(Color.FromRgb(220, 155, 40));
                var pen = new Pen(background, 2) { LineJoin = PenLineJoin.Bevel };
                drawingContext.DrawPolygon(background, pen, points);
                var exclamationPen = new Pen(new SolidColorBrush(Color.FromRgb(251, 245, 233)), 2);
                drawingContext.DrawLine(exclamationPen, new Point(10.5, 6), new Point(10.5, 14));
                drawingContext.DrawLine(exclamationPen, new Point(10.5, 16), new Point(10.5, 18));
            }
            else if (IconType == IconType.Error)
            {
                var pen = new Pen(new SolidColorBrush(Color.FromRgb(230, 80, 80)), 4);
                drawingContext.DrawLine(pen, new Point(2, 2), new Point(18, 18));
                drawingContext.DrawLine(pen, new Point(18, 2), new Point(2, 18));
            }
        }
    }
}