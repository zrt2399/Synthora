using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class CommandBarContentMarginConverter : IMultiValueConverter
    {
        public static CommandBarContentMarginConverter Instance { get; } = new CommandBarContentMarginConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count > 1 && values[0] is Thickness thickness && values[1] is int count)
            {
                return count == 0 ? new Thickness(0) : new Thickness(0, 0, thickness.Right, 0);
            }
            return new Thickness(0);
        }
    }
}