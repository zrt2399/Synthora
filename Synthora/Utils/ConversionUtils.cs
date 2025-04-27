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
    public static class ConversionUtils
    {
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
        /// 计算内层 Border 的 CornerRadius，使内外圆角在边框厚度作用下完美贴合。
        /// </summary>
        /// <param name="outerRadius">外层 CornerRadius（各角相同）。</param>
        /// <param name="outerBorderThickness">外层 BorderThickness（单侧宽度）。</param>
        /// <param name="innerBorderThickness">内层 BorderThickness（单侧宽度，若无则传0）。</param>
        /// <returns>内层 CornerRadius（各角相同，最小为0）。</returns>
        public static double CalcInnerRadius(double outerRadius, double outerBorderThickness, double innerBorderThickness = 0)
        {
            // 通用公式：R' = R - T/2 - T'/2
            double result = outerRadius - outerBorderThickness / 2.0 - innerBorderThickness / 2.0;

            // 保证不为负
            return Math.Max(0, result);
        }

        public static Geometry? DrawPolygon(this DrawingContext dc, Brush brush, Pen pen, params Point[] points)
        {
            if (!points.Any())
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