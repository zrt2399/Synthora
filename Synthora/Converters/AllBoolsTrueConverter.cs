using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class AllBoolsTrueConverter : IMultiValueConverter
    {
        public static AllBoolsTrueConverter Instance { get; } = new AllBoolsTrueConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values != null)
            {
                return values.OfType<bool>().All(x => x);
            }
            return false;
        }
    }

    public class AnyBoolsTrueConverter : IMultiValueConverter
    {
        public static AnyBoolsTrueConverter Instance { get; } = new AnyBoolsTrueConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values != null)
            {
                return values.OfType<bool>().Any(x => x);
            }
            return false;
        }
    }
}