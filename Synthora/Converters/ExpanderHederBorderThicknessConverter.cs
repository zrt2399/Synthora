using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class ExpanderHederBorderThicknessConverter : IMultiValueConverter
    {
        public static ExpanderHederBorderThicknessConverter Instance { get; } = new ExpanderHederBorderThicknessConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values != null && values.Count == 3 && values[0] is Thickness thickness && values[1] is ExpandDirection expandDirection && values[2] is bool isExpanded)
            {
                if (!isExpanded)
                {
                    return new Thickness(0);
                }
                return expandDirection switch
                {
                    ExpandDirection.Right => new Thickness(0, 0, thickness.Right, 0),
                    ExpandDirection.Left => new Thickness(thickness.Left, 0, 0, 0),
                    ExpandDirection.Up => new Thickness(0, thickness.Top, 0, 0),
                    ExpandDirection.Down => new Thickness(0, 0, 0, thickness.Bottom),
                    _ => new Thickness(0)
                };
            }
            return BindingOperations.DoNothing;
        }
    }
}