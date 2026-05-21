using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    public class ThemeModeToPathDataConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (Application.Current is { } application && value is ThemeMode themeMode)
            {
                if (application.TryGetResource($"ThemeMode{themeMode}Data", application.ActualThemeVariant, out var pathData))
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