using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthora.Converters
{
    public static class SynthoraConverters
    {
        public static EqualsConverter Equal { get; } = new EqualsConverter();
        public static NotEqualsConverter NotEqual { get; } = new NotEqualsConverter();
        public static NullOrEmptyToBoolConverter IsNullOrEmpty { get; } = new NullOrEmptyToBoolConverter();
        public static NotNullOrEmptyToBoolConverter IsNotNullOrEmpty { get; } = new NotNullOrEmptyToBoolConverter();
        public static HasFlagConverter HasFlag { get; } = new HasFlagConverter();
        public static IsZeroConverter IsZero { get; } = new IsZeroConverter();
        public static IsNotZeroConverter IsNotZero { get; } = new IsNotZeroConverter();
        public static BoxShadowConverter BoxShadowConverter { get; } = new BoxShadowConverter();
        public static ContrastColorConverter ContrastColorConverter { get; } = new ContrastColorConverter();
        public static BorderCornerRadiusConverter BorderCornerRadiusConverter { get; } = new BorderCornerRadiusConverter();
        public static SolidColorBrushToColorConverter SolidColorBrushToColor { get; } = new SolidColorBrushToColorConverter();
    }
}