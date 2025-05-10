using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

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
        /// Converts an Avalonia.Media.SolidColorBrush (or Color) to a contrasting brush of either Black or White.
        /// </summary>
        /// <param name="value">The background color, as a SolidColorBrush or Color.</param>
        /// <param name="targetType">The target type (Brush).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>SolidColorBrush of White or Black.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Color bgColor;

            switch (value)
            {
                case ISolidColorBrush scb:
                    bgColor = scb.Color;
                    break;
                case Color c:
                    bgColor = c;
                    break;
                default:
                    return AvaloniaProperty.UnsetValue;
            }

            // Calculate relative luminance per WCAG
            double r = bgColor.R / 255.0;
            double g = bgColor.G / 255.0;
            double b = bgColor.B / 255.0;

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
            throw new NotSupportedException();
        }
    }
}