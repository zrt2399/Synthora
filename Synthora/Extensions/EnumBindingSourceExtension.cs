using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Markup.Xaml;

namespace Synthora.Extensions
{
    /// <summary>
    /// Provides enum values for binding in XAML.
    /// Potentially incompatible with Native AOT.
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
        private Type? _enumType;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
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

        public EnumBindingSourceExtension([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type enumType) : this()
        {
            EnumType = enumType;
        }

        public bool IgnoreZero { get; set; }

        public bool UseKeyValuePair { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(EnumType) ?? EnumType;
            string[] names = Enum.GetNames(actualEnumType);
            Array enumValues = Enum.GetValuesAsUnderlyingType(actualEnumType);
            object?[] boxedValues = new object?[enumValues.Length + (actualEnumType == EnumType ? 0 : 1)];

            int offset = 0;
            if (actualEnumType != EnumType)
            {
                boxedValues[0] = null;
                offset = 1;
            }

            for (int i = 0; i < enumValues.Length; i++)
            {
                string name = names[i];
                object rawValue = enumValues.GetValue(i)!;
                object value = Enum.ToObject(actualEnumType, rawValue);

                boxedValues[i + offset] = UseKeyValuePair ? new KeyValuePair<string, object>(name, value) : value;
            }

            if (IgnoreZero)
            {
                return boxedValues.Where(x => Convert.ToInt64(x) != 0);
            }

            return boxedValues;
        }
    }
}