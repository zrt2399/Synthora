using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class HasFlagConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            try
            {
                var enumValue = (Enum)value;
                var flag = (Enum)Enum.Parse(value.GetType(), parameter?.ToString() ?? string.Empty);

                return enumValue.HasFlag(flag);
            }
            catch
            {
                return false;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}