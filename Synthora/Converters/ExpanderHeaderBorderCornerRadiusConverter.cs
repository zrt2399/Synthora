using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal class ExpanderHeaderBorderCornerRadiusConverter : IMultiValueConverter
    {
        public static ExpanderHeaderBorderCornerRadiusConverter Instance { get; } = new ExpanderHeaderBorderCornerRadiusConverter();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count == 3 && values[0] is CornerRadius cornerRadius && values[1] is ExpandDirection expandDirection && values[2] is bool isExpanded)
            {
                if (!isExpanded)
                {
                    return cornerRadius;
                }
                return expandDirection switch
                {
                    ExpandDirection.Up => new CornerRadius(0, 0, cornerRadius.BottomRight, cornerRadius.BottomLeft),
                    ExpandDirection.Down => new CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, 0, 0),
                    ExpandDirection.Left => new CornerRadius(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0),
                    ExpandDirection.Right => new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft),
                    _ => cornerRadius
                };
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}