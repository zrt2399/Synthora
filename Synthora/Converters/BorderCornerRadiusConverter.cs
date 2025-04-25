using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

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

    public class BorderCornerRadiusConverter : IValueConverter
    {
        public static BorderCornerRadiusConverter Instance { get; } = new BorderCornerRadiusConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
            {
                CornerRadiusType cornerRadiusType = CornerRadiusType.None;
                if (parameter is CornerRadiusType type)
                {
                    cornerRadiusType = type;
                }
                if (parameter is string stringCornerRadiusType)
                {
                    cornerRadiusType = Enum.Parse<CornerRadiusType>(stringCornerRadiusType);
                }
                if (cornerRadiusType != CornerRadiusType.None)
                {
                    return new CornerRadius( 
                        cornerRadiusType.HasFlag(CornerRadiusType.TopLeft) ? cornerRadius.TopLeft : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.TopRight) ? cornerRadius.TopRight : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomRight) ? cornerRadius.BottomRight : 0,
                        cornerRadiusType.HasFlag(CornerRadiusType.BottomLeft) ? cornerRadius.BottomLeft : 0);
                }
            }
            return new CornerRadius(0);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}