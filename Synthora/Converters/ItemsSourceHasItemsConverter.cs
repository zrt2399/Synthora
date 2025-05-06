using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class ItemsSourceHasItemsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<object> enumerable)
            {
                return enumerable.Any();
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 