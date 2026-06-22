using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class TextBlockHasContentConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count < 2)
            {
                return false;
            }

            return !string.IsNullOrEmpty(values[0] as string) || values[1] is > 0;
        }
    }
}