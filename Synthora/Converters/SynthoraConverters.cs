using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthora.Converters
{
    public static class SynthoraConverters
    {
        public static NullOrEmptyToBoolConverter IsNullOrEmpty { get; } = new NullOrEmptyToBoolConverter();
        public static NotNullOrEmptyToBoolConverter IsNotNullOrEmpty { get; } = new NotNullOrEmptyToBoolConverter();
        public static AllBoolsTrueConverter IsAllTrue { get; } = new AllBoolsTrueConverter();
        public static AnyBoolsTrueConverter IsAnyTrue { get; } = new AnyBoolsTrueConverter();
        public static BorderCornerRadiusConverter BorderCornerRadius { get; } = new BorderCornerRadiusConverter();
    }
}