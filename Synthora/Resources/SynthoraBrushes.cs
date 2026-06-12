using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;

namespace Synthora.Resources
{
    /// <summary>
    /// Provides cached brushes resolved from Synthora theme resources.
    /// </summary>
    public static class SynthoraBrushes
    {
        static SynthoraBrushes()
        {
            IconForeground = Brushes.White;
            InformationBrush = GetBrush("PrimaryBrush");
            QuestionBrush = GetBrush(nameof(QuestionBrush));
            SuccessBrush = GetBrush(nameof(SuccessBrush));
            WarningBrush = GetBrush(nameof(WarningBrush));
            ErrorBrush = GetBrush(nameof(ErrorBrush));
            InformationTipBackgroundBrush = GetBrush(nameof(InformationTipBackgroundBrush));
            QuestionTipBackgroundBrush = GetBrush(nameof(QuestionTipBackgroundBrush));
            SuccessTipBackgroundBrush = GetBrush(nameof(SuccessTipBackgroundBrush));
            WarningTipBackgroundBrush = GetBrush(nameof(WarningTipBackgroundBrush));
            DangerTipBackgroundBrush = GetBrush(nameof(DangerTipBackgroundBrush));
            ErrorTipBackgroundBrush = GetBrush(nameof(ErrorTipBackgroundBrush));
        }

        public static IImmutableSolidColorBrush IconForeground { get; }

        public static IImmutableSolidColorBrush InformationBrush { get; }

        public static IImmutableSolidColorBrush QuestionBrush { get; }

        public static IImmutableSolidColorBrush SuccessBrush { get; }

        public static IImmutableSolidColorBrush WarningBrush { get; }

        public static IImmutableSolidColorBrush ErrorBrush { get; }

        public static IImmutableSolidColorBrush InformationTipBackgroundBrush { get; }

        public static IImmutableSolidColorBrush QuestionTipBackgroundBrush { get; }

        public static IImmutableSolidColorBrush SuccessTipBackgroundBrush { get; }

        public static IImmutableSolidColorBrush WarningTipBackgroundBrush { get; }

        public static IImmutableSolidColorBrush DangerTipBackgroundBrush { get; }

        public static ISolidColorBrush ErrorTipBackgroundBrush { get; }

        private static IImmutableSolidColorBrush GetBrush(string resourceKey)
        {
            if (Application.Current is { } application &&
                application.TryGetResource(resourceKey, ThemeVariant.Default, out var value) &&
                value is ISolidColorBrush brush)
            {
                return new ImmutableSolidColorBrush(brush);
            }

            return Brushes.White;
        }
    }
}