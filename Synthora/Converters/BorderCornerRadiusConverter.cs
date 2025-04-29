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
            if (values.Count >= 2 && values[0] is Thickness thickness && values[1] is CornerRadius cornerRadius)
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
                    double innerLeft = 0, innerRight = 0, innerTop = 0, innerBottom = 0;
                    if (values.Count == 3)
                    {
                        if (values[2] is Thickness innerThickness)
                        {
                            innerLeft = innerThickness.Left;
                            innerRight = innerThickness.Right;
                            innerTop = innerThickness.Top;
                            innerBottom = innerThickness.Bottom;
                        }
                        else if (double.TryParse(values[2]?.ToString(), out var result))
                        {
                            innerLeft = innerRight = innerTop = innerBottom = result;
                        }
                    }

                    return new CornerRadius(
                        cornerRadiusType.HasFlag(CornerRadiusType.TopLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopLeft, thickness.Left, innerLeft) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.TopRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopRight, thickness.Top, innerTop) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomRight, thickness.Right, innerRight) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom, innerBottom) : 0);
                }
                return new CornerRadius(0);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}