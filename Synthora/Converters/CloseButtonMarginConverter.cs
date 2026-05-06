using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Synthora.Converters
{
    internal class CloseButtonMarginConverter : IValueConverter
    {
        public static CloseButtonMarginConverter Instance { get; } = new CloseButtonMarginConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is true)
            {
                return new Thickness(0, 0, 16, 0);
            }
            return new Thickness(0);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
} 