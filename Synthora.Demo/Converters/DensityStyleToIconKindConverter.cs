using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Material.Icons;

namespace Synthora.Demo.Converters
{
    public class DensityStyleToIconKindConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                DensityStyle.Compact => MaterialIconKind.ArrowCollapseVertical,
                DensityStyle.Normal => MaterialIconKind.ArrowExpandVertical, 
                _ => MaterialIconKind.HelpCircleOutline
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}