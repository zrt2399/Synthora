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
        public static EnumDescriptionConverter EnumDescriptionConverter { get; } = new EnumDescriptionConverter();
        public static BorderCornerRadiusConverter BorderCornerRadiusConverter { get; } = new BorderCornerRadiusConverter();
        public static DoubleToGridLengthConverter DoubleToGridLengthConverter { get; } = new DoubleToGridLengthConverter();
        public static DoubleToCornerRadiusConverter DoubleToCornerRadiusConverter { get; } = new DoubleToCornerRadiusConverter();
        public static ThemeModeToPathDataConverter ThemeModeToPathDataConverter { get; } = new ThemeModeToPathDataConverter();
        public static ThemeDensityToPathDataConverter ThemeDensityToPathDataConverter { get; } = new ThemeDensityToPathDataConverter();
        public static SolidColorBrushToColorConverter SolidColorBrushToColorConverter { get; } = new SolidColorBrushToColorConverter();
    }
}