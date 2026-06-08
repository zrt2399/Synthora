using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Avalonia.Data.Converters;

namespace Synthora.Converters
{
    /// <summary>
    /// Converts an enum value to its description text.
    /// Potentially incompatible with Native AOT.
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        [UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "The 'parameter' from XAML is a safe Type, but IValueConverter signature erases its AOT annotations.")]
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return parameter is Type enumType ? GetEnumDescSafe(value, enumType) : GetEnumDescUnsafe(value);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string GetEnumDescSafe(object obj, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type enumType)
        {
            string name = obj.ToString() ?? string.Empty;
            return ExtractDescription(name, enumType);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Fallback for non-AOT. Passing unannotated actualType to annotated parameter is expected here.")]
        public static string GetEnumDescUnsafe(object obj)
        {
            Type actualType = obj.GetType();
            string name = obj.ToString() ?? string.Empty;
            return ExtractDescription(name, actualType);
        }

        private static string ExtractDescription(string name, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type)
        {
            var field = type.GetField(name);
            if (field?.GetCustomAttribute<DescriptionAttribute>() is { } attribute)
            {
                return attribute.Description;
            }

            return name;
        }
    }

    /// <summary>
    /// A custom enum type converter that converts enum values
    /// to their description strings using the <see cref="DescriptionAttribute"/>.
    /// Potentially incompatible with Native AOT.
    /// </summary>
    /// <param name="type">The enum type to convert.</param>
    public class EnumDescriptionTypeConverter([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type) : EnumConverter(type)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
        private readonly Type _enumType = type;

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string name = value?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(name))
                {
                    return string.Empty;
                }

                var field = _enumType.GetField(name);
                if (field?.GetCustomAttribute<DescriptionAttribute>() is { } attribute)
                {
                    return attribute.Description;
                }
                return name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}