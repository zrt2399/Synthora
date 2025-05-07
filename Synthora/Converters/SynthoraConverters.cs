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
        public static IsZeroConverter IsZero { get; } = new IsZeroConverter();
        public static IsNotZeroConverter IsNotZero { get; } = new IsNotZeroConverter();
        public static ItemsSourceHasItemsConverter HasItems { get; } = new ItemsSourceHasItemsConverter();
        public static BoxShadowConverter BoxShadowConverter { get; } = new BoxShadowConverter();
        public static BorderCornerRadiusConverter BorderCornerRadiusConverter { get; } = new BorderCornerRadiusConverter();
    }
}