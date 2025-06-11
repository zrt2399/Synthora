using System;
using System.Linq;
using Avalonia.Markup.Xaml;

namespace Synthora.Extensions
{
    /// <summary>
    /// Provides enum values for binding in XAML.
    /// Uses reflection (e.g., Enum.GetValues), and is not compatible with NativeAOT.
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type? _enumType;
        public Type? EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (value != null)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must be an enum.", nameof(value));
                        }
                    }
                    _enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType) : this()
        {
            EnumType = enumType;
        }

        public EnumBindingSourceExtension(Type enumType, bool ignoreZero) : this(enumType)
        {
            IgnoreZero = ignoreZero;
        }

        public bool IgnoreZero { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(EnumType) ?? EnumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == EnumType)
            {
                if (IgnoreZero)
                {
                    return enumValues.Cast<object>().Where(x => Convert.ToInt64(x) != 0);
                }
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}