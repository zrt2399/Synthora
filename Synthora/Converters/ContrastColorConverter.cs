using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;

namespace Synthora.Converters
{
    /// <summary>
    /// Converter that returns either White or Black based on the luminance of the provided background color.
    /// </summary>
    public class ContrastColorConverter : IValueConverter
    {
        public IBrush WhiteBrush { get; set; } = Brushes.White;
        public IBrush BlackBrush { get; set; } = Brushes.Black;

        /// <summary>
        /// Converts a <see cref="SolidColorBrush"/> or <see cref="Color"/> to a contrasting Black or White brush.
        /// </summary>
        /// <param name="value">The background color, provided as a <see cref="SolidColorBrush"/> or <see cref="Color"/>.</param>
        /// <param name="targetType">The target type (expected to be Brush).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>A <see cref="SolidColorBrush"/> that is either Black or White.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Color? bgColor = value switch
            {
                ISolidColorBrush scb => scb.Color,
                Color c => c,
                _ => null
            };

            if (bgColor == null || bgColor.Value.A == 0)
            {
                var variant = Application.Current?.ActualThemeVariant;
                // Dark theme → return White; Light (or default) theme → return Black
                return variant == ThemeVariant.Dark ? WhiteBrush : BlackBrush;
            }

            // Calculate relative luminance per WCAG
            double r = bgColor.Value.R / 255.0;
            double g = bgColor.Value.G / 255.0;
            double b = bgColor.Value.B / 255.0;

            // Apply gamma correction
            r = (r <= 0.03928) ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
            g = (g <= 0.03928) ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
            b = (b <= 0.03928) ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

            // Luminance
            double luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;

            // Return White for dark backgrounds (luminance < 0.5), else Black
            return luminance < 0.5 ? WhiteBrush : BlackBrush;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}