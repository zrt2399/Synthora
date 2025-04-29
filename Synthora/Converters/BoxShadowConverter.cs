using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Synthora.Converters
{
    public class BoxShadowConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ISolidColorBrush solidColorBrush)
            {
                if (parameter is string param && !string.IsNullOrEmpty(param))
                {
                    var boxShadow = BoxShadow.Parse(param);
                    return new BoxShadows(new BoxShadow()
                    {
                        IsInset = boxShadow.IsInset,
                        OffsetX = boxShadow.OffsetX,
                        OffsetY = boxShadow.OffsetY,
                        Blur = boxShadow.Blur,
                        Spread = boxShadow.Spread,
                        Color = solidColorBrush.Color
                    });
                }
                return new BoxShadows(new BoxShadow() { Blur = 8, Color = solidColorBrush.Color, });
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}