using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class DateTimePickerPopupMarginConverter : IValueConverter
    {
        public static DateTimePickerPopupMarginConverter Instance { get; } = new DateTimePickerPopupMarginConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return new Thickness(thickness.Right, 0, 0, 0);
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}