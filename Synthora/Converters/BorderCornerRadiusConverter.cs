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
                var cornerRadiusType = CornerRadiusType.None;
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
                    double innerLeft = 0, innerRight = 0, innerTop = 0, innerBottom = 0;
                    if (values.Count > 2 && values[2] is Thickness outerPadding)
                    {
                        outerLeft = outerPadding.Left;
                        outerRight = outerPadding.Right;
                        outerTop = outerPadding.Top;
                        outerBottom = outerPadding.Bottom;
                    }

                    if (values.Count > 3 && values[3] is Thickness innerMargin)
                    {
                        outerLeft += innerMargin.Left;
                        outerRight += innerMargin.Right;
                        outerTop += innerMargin.Top;
                        outerBottom += innerMargin.Bottom;
                    }

                    if (values.Count > 4 && values[4] is Thickness innerThickness)
                    {
                        innerLeft = innerThickness.Left;
                        innerRight = innerThickness.Right;
                        innerTop = innerThickness.Top;
                        innerBottom = innerThickness.Bottom;
                    }

                    return new CornerRadius(
                        cornerRadiusType.HasFlag(CornerRadiusType.TopLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopLeft, thickness.Left, outerLeft, innerLeft) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.TopRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopRight, thickness.Top, outerTop, innerTop) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomRight, thickness.Right, outerRight, innerRight) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom, outerBottom, innerBottom) : 0);
                }
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}