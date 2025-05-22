using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class HeaderDividerHeightConverter : IValueConverter
    {
        public static HeaderDividerHeightConverter Instance { get; } = new HeaderDividerHeightConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                return new Thickness(0, 0, 0, height);
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}