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