using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    internal sealed class DataGridVerticalGridLinesVisibilityConverter : IValueConverter
    {
        public static DataGridVerticalGridLinesVisibilityConverter Instance { get; } = new DataGridVerticalGridLinesVisibilityConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is DataGridGridLinesVisibility.Vertical or DataGridGridLinesVisibility.All;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}