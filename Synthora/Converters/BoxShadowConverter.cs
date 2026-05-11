using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Synthora.Converters
{
    public class BoxShadowConverter : IValueConverter
    {
        public const string DefaultParameter = "0 0 4 0 #00000000";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Color color;
            switch (value)
            {
                case ISolidColorBrush solidColorBrush:
                    color = solidColorBrush.Color;
                    break;
                case Color c:
                    color = c;
                    break;
                default:
                    return new BoxShadows();
            }

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
                    Color = color
                });
            }
            return new BoxShadows(new BoxShadow()
            {
                Blur = 8,
                Color = color,
            });
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}