using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class EqualsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
            {
                return true;
            }

            if (value == null || parameter == null)
            {
                return false;
            }
            return value.Equals(parameter);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool?)value == true ? parameter : BindingOperations.DoNothing;
        }
    }

    public class NotEqualsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
            {
                return false;
            }

            if (value == null || parameter == null)
            {
                return true;
            }
            return !value.Equals(parameter);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool?)value != true ? parameter : BindingOperations.DoNothing;
        }
    }
}