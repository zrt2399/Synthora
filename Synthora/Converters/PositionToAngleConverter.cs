using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class PositionToAngleConverter : IValueConverter
    {
        public static PositionToAngleConverter Instance { get; } = new PositionToAngleConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is double d ? d * 3.6 : AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is double d ? d / 3.6 : AvaloniaProperty.UnsetValue;
        }
    }
}