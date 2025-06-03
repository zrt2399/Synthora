using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public static string ToHexString(this IEnumerable<byte> bytes, string separator = " ")
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            var count = bytes.Count();
            if (count == 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                stringBuilder.Append(bytes.ElementAt(i).ToString("X2"));
                if (i < count - 1)
                {
                    stringBuilder.Append(separator);
                }
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
            StringBuilder stringBuilder = new StringBuilder();
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
        public static string GetDescription<T>(this T enumerationValue) where T : Enum
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
        /// <param name="innerBorderThickness">
        /// Optional thickness of an inner border. Defaults to 0 if not specified.
        /// </param>
        /// <returns>
        /// The computed inner radius after subtracting half of each border's thickness;
        /// returns 0 if the computed value would be negative.
        /// </returns>
        public static double CalcInnerRadius(double outerRadius, double outerBorderThickness, double innerBorderThickness = 0)
        {
            //R' = R - T/2 - T'/2
            double result = outerRadius - (outerBorderThickness / 2.0) - (innerBorderThickness / 2.0);
 
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