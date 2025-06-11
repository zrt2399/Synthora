using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class DataGridLastColumnVisibleConverter : IMultiValueConverter
    {
        public static DataGridLastColumnVisibleConverter Instance { get; } = new DataGridLastColumnVisibleConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count > 1 && values[0] is Thickness thickness && values[1] is bool and true)
            {
                return thickness;
            }
            return new Thickness(0);
        }
    }
}