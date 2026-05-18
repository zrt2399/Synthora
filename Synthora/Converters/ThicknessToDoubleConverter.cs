using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal enum Thicknesses
    {
        Left,
        Top,
        Right,
        Bottom
    }

    internal class ThicknessToDoubleConverter : IValueConverter
    {
        public static ThicknessToDoubleConverter Instance { get; } = new ThicknessToDoubleConverter();
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Thickness thickness && parameter is Thicknesses thicknesses)
            {
                return thicknesses switch
                {
                    Thicknesses.Left => thickness.Left,
                    Thicknesses.Top => thickness.Top,
                    Thicknesses.Right => thickness.Right,
                    Thicknesses.Bottom => thickness.Bottom,
                    _ => throw new InvalidOperationException("Invalid Thicknesses value.")
                };
            }
            return 0d;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}