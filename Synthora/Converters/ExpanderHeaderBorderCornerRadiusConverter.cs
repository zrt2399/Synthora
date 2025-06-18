using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Synthora.Utils;

namespace Synthora.Converters
{
    internal class ExpanderHeaderBorderCornerRadiusConverter : IMultiValueConverter
    {
        public static ExpanderHeaderBorderCornerRadiusConverter Instance { get; } = new ExpanderHeaderBorderCornerRadiusConverter();

        private static double CalcInnerRadius(double outerRadius, double outerBorderThickness, double innerBorderThickness = 0)
        {
            return ConversionUtils.CalcInnerRadius(outerRadius, outerBorderThickness, innerBorderThickness);
        }

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count > 3 && values[0] is CornerRadius cornerRadius && values[1] is Thickness thickness && values[2] is ExpandDirection expandDirection && values[3] is bool isExpanded)
            {
                if (!isExpanded)
                {
                    return new CornerRadius(
                        CalcInnerRadius(cornerRadius.TopLeft, thickness.Left),
                        CalcInnerRadius(cornerRadius.TopRight, thickness.Top),
                        CalcInnerRadius(cornerRadius.BottomRight, thickness.Right),
                        CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom));
                }
                return expandDirection switch
                {
                    ExpandDirection.Up => new CornerRadius(0, 0, CalcInnerRadius(cornerRadius.BottomRight, thickness.Right), CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom)),
                    ExpandDirection.Down => new CornerRadius(CalcInnerRadius(cornerRadius.TopLeft, thickness.Left), CalcInnerRadius(cornerRadius.TopRight, thickness.Top), 0, 0),
                    ExpandDirection.Left => new CornerRadius(0, CalcInnerRadius(cornerRadius.TopRight, thickness.Top), CalcInnerRadius(cornerRadius.BottomRight, thickness.Right), 0),
                    ExpandDirection.Right => new CornerRadius(CalcInnerRadius(cornerRadius.TopLeft, thickness.Left), 0, 0, CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom)),
                    _ => AvaloniaProperty.UnsetValue
                };
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}