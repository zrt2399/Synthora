using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class DensityStyleToIconKindConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (Application.Current is { } application && value is DensityStyle densityStyle)
            {
                if (application.TryGetResource($"DensityStyle{densityStyle}Data", application.ActualThemeVariant, out var pathData))
                {
                    return pathData;
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