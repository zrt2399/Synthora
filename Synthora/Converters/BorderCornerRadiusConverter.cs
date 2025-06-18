using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Synthora.Utils;

namespace Synthora.Converters
{
    [Flags]
    public enum CornerRadiusType
    {
        None = 0,
        TopLeft = 1,
        TopRight = 1 << 1,
        BottomRight = 1 << 2,
        BottomLeft = 1 << 3,
        All = TopLeft | TopRight | BottomRight | BottomLeft
    }

    public class BorderCornerRadiusConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count > 1 && values[0] is CornerRadius cornerRadius && values[1] is Thickness thickness)
            {
                CornerRadiusType cornerRadiusType = CornerRadiusType.None;
                if (parameter is CornerRadiusType type)
                {
                    cornerRadiusType = type;
                }
                else if (parameter is string stringCornerRadiusType)
                {
                    cornerRadiusType = Enum.Parse<CornerRadiusType>(stringCornerRadiusType);
                }
                if (cornerRadiusType != CornerRadiusType.None)
                {
                    double outerLeft = 0, outerRight = 0, outerTop = 0, outerBottom = 0;
                    if (values.Count == 3)
                    {
                        if (values[2] is Thickness outerPadding)
                        {
                            outerLeft = outerPadding.Left;
                            outerRight = outerPadding.Right;
                            outerTop = outerPadding.Top;
                            outerBottom = outerPadding.Bottom;
                        }
                        else if (double.TryParse(values[2]?.ToString(), out var result))
                        {
                            outerLeft = outerRight = outerTop = outerBottom = result;
                        }
                    }

                    return new CornerRadius(
                        cornerRadiusType.HasFlag(CornerRadiusType.TopLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopLeft, thickness.Left, outerLeft) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.TopRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopRight, thickness.Top, outerTop) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomRight, thickness.Right, outerRight) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom, outerBottom) : 0);
                }
                return new CornerRadius(0);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}