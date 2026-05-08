using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Material.Icons;
using Synthora.Demo.ViewModels;

namespace Synthora.Demo.Converters
{
    public class AppThemeModeToIconKindConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                AppThemeMode.Default => MaterialIconKind.Monitor,
                AppThemeMode.Light => MaterialIconKind.WhiteBalanceSunny,
                AppThemeMode.Dark => MaterialIconKind.WeatherNight,
                _ => MaterialIconKind.HelpCircleOutline
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}