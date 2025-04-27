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
            if (values.Count == 2 && values[0] is Thickness thickness && values[1] is CornerRadius cornerRadius)
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
                    return new CornerRadius(
                        cornerRadiusType.HasFlag(CornerRadiusType.TopLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopLeft, thickness.Left) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.TopRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.TopRight, thickness.Top) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomRight) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomRight, thickness.Right) : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomLeft) ? ConversionUtils.CalcInnerRadius(cornerRadius.BottomLeft, thickness.Bottom) : 0);
                }
                return new CornerRadius(0);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}