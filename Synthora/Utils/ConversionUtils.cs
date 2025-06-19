using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Avalonia;
using Avalonia.Media;

namespace Synthora.Utils
{
    /// <summary>
    /// Provides utility methods for converting data.
    /// </summary>
    public static class ConversionUtils
    {
        /// <summary>
        /// Converts a sequence of bytes to a hexadecimal string, with an optional separator between byte values.
        /// </summary>
        public static string ToHexString(this IEnumerable<byte> bytes, string? separator = " ")
        {
            ArgumentNullException.ThrowIfNull(bytes);

            using var enumerator = bytes.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return string.Empty;
            }

            const string format = "X2";
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(enumerator.Current.ToString(format));

            while (enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(separator))
                {
                    stringBuilder.Append(separator);
                }
                stringBuilder.Append(enumerator.Current.ToString(format));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts the given string to its hexadecimal representation using UTF-8 encoding.
        /// </summary>
        public static string ToHash(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts a hexadecimal string back to its original text assuming UTF-8 encoding.
        /// </summary>
        public static string FromHash(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            try
            {
                byte[] data = new byte[str.Length / 2];
                for (int i = 0; i < str.Length; i += 2)
                {
                    data[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
                }
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Retrieves the <see cref="DescriptionAttribute"/> text for an enum value, 
        /// or the enum's name if no description is found.
        /// </summary>
        public static string GetDescription<T>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this T enumerationValue) where T : Enum
        {
            Type type = enumerationValue.GetType();

            if (type.GetField(enumerationValue.ToString())?.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }
            return enumerationValue.ToString();
        }

        /// <summary>
        /// Calculates the inner radius of a shape given its outer radius and border thickness values.
        /// </summary>
        /// <param name="outerRadius">The outer radius of the shape.</param>
        /// <param name="outerBorderThickness">The thickness of the outer border.</param>
        /// <param name="outerBorderPadding">
        /// Optional padding of an outer border. Defaults to 0 if not specified.
        /// </param>
        /// <returns>
        /// The computed inner radius after subtracting half of each border's thickness;
        /// returns 0 if the computed value would be negative.
        /// </returns>
        public static double CalcInnerRadius(double outerRadius, double outerBorderThickness, double outerBorderPadding = 0)
        {
            double result;
            if (outerBorderPadding < 0.5)
            {
                // R' = R - T/2 - T'/2
                result = outerRadius - outerBorderThickness / 2 - outerBorderPadding / 2;
            }
            else
            {
                var shrink = outerBorderThickness + outerBorderPadding;
                if (outerBorderPadding >= outerRadius || shrink >= outerRadius)
                {
                    result = outerRadius / 2 - outerBorderThickness / 2;
                }
                else
                {
                    // result = outerRadius - outerBorderPadding /*- outerBorderThickness / 2*/;
                    // result = outerRadius - 0.7 * (outerBorderPadding + outerBorderThickness);
                    result = outerRadius - shrink * Math.Sqrt(2) / 2;
                }
            }

            return Math.Max(0, result);
        }

        /// <summary>
        /// Draws a polygon defined by a sequence of points onto the specified <see cref="DrawingContext"/>.
        /// </summary>
        public static Geometry? DrawPolygon(this DrawingContext dc, Brush brush, Pen pen, params Point[] points)
        {
            if (points.Length == 0)
            {
                return null;
            }

            var geometry = new StreamGeometry();
            using (var geometryContext = geometry.Open())
            {
                geometryContext.BeginFigure(points[0], true);
                for (var i = 1; i < points.Length; i++)
                {
                    geometryContext.LineTo(points[i]);
                }
            }
            //geometry.Freeze();
            dc.DrawGeometry(brush, pen, geometry);
            return geometry;
        }
    }
}