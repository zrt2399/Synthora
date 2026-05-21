using System;
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
        public static string ToHexString(this byte[] bytes, string separator = " ")
        {
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(separator))
            {
                return Convert.ToHexString(bytes);
            }

            // 提前计算最终字符串的精确长度：(字节数 * 2) + (分隔符长度 * (字节数 - 1))
            int sepLen = separator.Length;
            int totalLen = bytes.Length * 2 + (bytes.Length - 1) * sepLen;

            // 终极大杀器：string.Create (直接在最终生成的字符串内存中作画，无任何中间对象)
            return string.Create(totalLen, (bytes, separator, sepLen), static (span, state) =>
            {
                (var data, var sep, var sepLength) = state;
                var sepSpan = sep.AsSpan();

                // 极速查表：编译器会将其优化为指向数据段的指针，没有任何内存分配
                ReadOnlySpan<char> hexMap = "0123456789ABCDEF";

                int pos = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    // 写入分隔符
                    if (i > 0)
                    {
                        sepSpan.CopyTo(span.Slice(pos));
                        pos += sepLength;
                    }

                    // 直接通过位运算提取高四位和低四位，查表写入，不产生任何临时字符串！
                    byte b = data[i];
                    span[pos++] = hexMap[b >> 4];
                    span[pos++] = hexMap[b & 0x0F];
                }
            });
        }

        /// <summary>
        /// Converts a hexadecimal string into a sequence of bytes.
        /// </summary>
        /// <param name="hexString">
        /// A hexadecimal string that may contain spaces between byte groups.
        /// Longer groups are split every two characters from left to right.
        /// </param>
        /// <returns>A sequence of bytes parsed from the hexadecimal string.</returns>
        public static byte[] ToBytes(this string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return [];
            }

            byte[] res = new byte[hexString.Length];
            int pos = 0;

            for (int i = 0; i < hexString.Length; i++)
            { 
                if (!char.IsAsciiHexDigit(hexString[i]))
                {
                    continue;
                }
 
                int v = (hexString[i] & 0xF) + ((hexString[i] >> 6) * 9);
 
                if (i + 1 < hexString.Length && char.IsAsciiHexDigit(hexString[i + 1]))
                {
                    v = (v << 4) | ((hexString[++i] & 0xF) + ((hexString[i] >> 6) * 9));
                }

                res[pos++] = (byte)v;
            }

            return res[..pos];
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
        /// Potentially incompatible with Native AOT.
        /// </summary>
        public static string GetDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>
            (this T t) where T : Enum
        {
            Type type = t.GetType();
            string enumName = t.ToString();

            if (type.GetField(enumName)?.GetCustomAttribute<DescriptionAttribute>() is { } descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }

            return enumName;
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