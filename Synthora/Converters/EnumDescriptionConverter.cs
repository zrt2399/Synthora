using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using Synthora.Utils;

namespace Synthora.Converters
{
    /// <summary>
    /// Converts an enum value to its description text.
    /// Potentially incompatible with Native AOT.
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return EnumDescriptionTypeConverter.GetEnumDesc(value);
        }

        /// <summary>
        /// Convert-back is not supported for this converter.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// Always thrown because reverse conversion is not implemented.
        /// </exception>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A custom enum type converter that converts enum values
    /// to their description strings using the <see cref="DescriptionAttribute"/>.
    /// Potentially incompatible with Native AOT.
    /// </summary>
    /// <param name="type">The enum type to convert.</param>
    public class EnumDescriptionTypeConverter(Type type) : EnumConverter(type)
    {
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return GetEnumDesc(value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Gets the description text for the specified enum value.
        /// Potentially incompatible with Native AOT.
        /// </summary>
        /// <param name="obj">The enum value.</param>
        /// <returns>
        /// The description text defined by <see cref="DescriptionAttribute"/>,
        /// or the enum name if no description exists.
        /// Returns an empty string if the value is null.
        /// </returns>
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