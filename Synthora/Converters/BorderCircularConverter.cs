using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class BorderCircularConverter : IMultiValueConverter
    {
        public static BorderCircularConverter Instance { get; } = new BorderCircularConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count == 2 && values[0] is double num && values[1] is double num2)
            {
                if (num < double.Epsilon || num2 < double.Epsilon)
                {
                    return default(CornerRadius);
                }

                return new CornerRadius(Math.Min(num, num2) / 2.0);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}