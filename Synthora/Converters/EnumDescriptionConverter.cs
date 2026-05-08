using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using Synthora.Utils;

namespace Synthora.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return EnumDescriptionTypeConverter.GetEnumDesc(value);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type) : base(type) { }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return GetEnumDesc(value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static string GetEnumDesc(object? obj)
        {
            if (obj is not Enum enumValue)
            {
                return obj?.ToString() ?? string.Empty;
            }

            return enumValue.GetDescription();
        }
    }
}