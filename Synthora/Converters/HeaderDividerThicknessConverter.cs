using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class HeaderDividerThicknessConverter : IMultiValueConverter
    {
        public static HeaderDividerThicknessConverter Instance { get; } = new HeaderDividerThicknessConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count > 1 && values[0] is Dock dock && values[1] is double thickness)
            {
                return dock switch
                {
                    Dock.Left => new Thickness(0, 0, thickness, 0),
                    Dock.Top => new Thickness(0, 0, 0, thickness),
                    Dock.Right => new Thickness(thickness, 0, 0, 0),
                    _ => new Thickness(0, thickness, 0, 0)
                };
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}