using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class ExpanderHeaderBorderThicknessConverter : IMultiValueConverter
    {
        public static ExpanderHeaderBorderThicknessConverter Instance { get; } = new ExpanderHeaderBorderThicknessConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count == 3 && values[0] is Thickness thickness && values[1] is ExpandDirection expandDirection && values[2] is bool isExpanded)
            {
                if (!isExpanded)
                {
                    return new Thickness(0);
                }
                return expandDirection switch
                {
                    ExpandDirection.Up => new Thickness(0, thickness.Top, 0, 0),
                    ExpandDirection.Down => new Thickness(0, 0, 0, thickness.Bottom),
                    ExpandDirection.Left => new Thickness(thickness.Left, 0, 0, 0),
                    ExpandDirection.Right => new Thickness(0, 0, thickness.Right, 0),
                    _ => new Thickness(0)
                };
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}