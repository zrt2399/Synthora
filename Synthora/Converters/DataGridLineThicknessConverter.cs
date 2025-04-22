using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace Synthora.Converters
{
    internal class DataGridLineThicknessConverter : IValueConverter
    {
        public static DataGridLineThicknessConverter Instance { get; } = new DataGridLineThicknessConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DataGridGridLinesVisibility visibility && parameter is Orientation orientation)
            {
                if (orientation == Orientation.Vertical)
                {
                    if (visibility == DataGridGridLinesVisibility.Vertical || visibility == DataGridGridLinesVisibility.All)
                    {
                        return new Thickness(0, 0, 1, 0);
                    }
                }
                else
                {
                    if (visibility == DataGridGridLinesVisibility.Horizontal || visibility == DataGridGridLinesVisibility.All)
                    {
                        return new Thickness(0, 0, 0, 1);
                    }
                }
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}