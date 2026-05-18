using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                double width => new GridLength(width, GridUnitType.Pixel),
                int width => new GridLength(width, GridUnitType.Pixel),
                _ => AvaloniaProperty.UnsetValue,
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                GridLength { IsAbsolute: true } width => width.Value,
                _ => AvaloniaProperty.UnsetValue,
            };
        }
    }
}