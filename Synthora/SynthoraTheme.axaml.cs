using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Synthora
{
    /// <summary>
    /// Defines theme modes that map to Avalonia <see cref="ThemeVariant"/> values.
    /// </summary>
    public enum ThemeMode
    {
        /// <summary>
        /// Maps to <see cref="ThemeVariant.Default"/>.
        /// </summary>
        [Description("系统")]
        Default,
        /// <summary>
        /// Maps to <see cref="ThemeVariant.Light"/>.
        /// </summary>
        [Description("浅色")]
        Light,
        /// <summary>
        /// Maps to <see cref="ThemeVariant.Dark"/>.
        /// </summary>
        [Description("深色")]
        Dark
    }

    /// <summary>
    /// Defines the density presets used by Synthora theme resources.
    /// </summary>
    public enum ThemeDensity
    {
        /// <summary>
        /// Uses compact density resources.
        /// </summary>
        [Description("紧凑")]
        Compact,
        /// <summary>
        /// Uses normal density resources.
        /// </summary>
        [Description("默认")]
        Normal,
        /// <summary>
        /// Uses spacious density resources.
        /// </summary>
        [Description("宽松")]
        Spacious
    }

    /// <summary>
    /// Defines the language resources used by Synthora theme strings.
    /// </summary>
    public enum ThemeLanguage
    {
        /// <summary>
        /// Uses English string resources.
        /// </summary>
        [Description("English")]
        English,
        /// <summary>
        /// Uses Simplified Chinese string resources.
        /// </summary>
        [Description("简体中文")]
        SimplifiedChinese,
        /// <summary>
        /// Uses Traditional Chinese string resources.
        /// </summary>
        [Description("繁體中文")]
        TraditionalChinese,
        /// <summary>
        /// Uses Japanese string resources.
        /// </summary>
        [Description("日本語")]
        Japanese,
        /// <summary>
        /// Uses Korean string resources.
        /// </summary>
        [Description("한국어")]
        Korean
    }

    /// <summary>
    /// Provides the Synthora theme resources and control styles for an Avalonia application.
    /// </summary>
    public class SynthoraTheme : Styles
    {
        internal const string DensityCompactResourceKey = "DensityCompact";
        internal const string DensityNormalResourceKey = "DensityNormal";
        internal const string DensitySpaciousResourceKey = "DensitySpacious";
        internal const string LanguageEnglishResourceKey = "LanguageEnglish";
        internal const string LanguageSimplifiedChineseResourceKey = "LanguageSimplifiedChinese";
        internal const string LanguageTraditionalChineseResourceKey = "LanguageTraditionalChinese";
        internal const string LanguageKoreanResourceKey = "LanguageKorean";
        internal const string LanguageJapaneseResourceKey = "LanguageJapanese";

        private ThemeDensity _themeDensity = ThemeDensity.Normal;
        private IResourceProvider? _currentDensityResource;
        private ThemeLanguage _themeLanguage = ThemeLanguage.English;
        private IResourceProvider? _currentLanguageResource;

        /// <summary>
        /// Defines the <see cref="ThemeDensity"/> property.
        /// </summary>
        public static readonly DirectProperty<SynthoraTheme, ThemeDensity> ThemeDensityProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, ThemeDensity>(
                nameof(ThemeDensity), o => o.ThemeDensity, (o, v) => o.ThemeDensity = v);

        /// <summary>
        /// Defines the <see cref="ThemeLanguage"/> property.
        /// </summary>
        public static readonly DirectProperty<SynthoraTheme, ThemeLanguage> ThemeLanguageProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, ThemeLanguage>(
                nameof(ThemeLanguage), o => o.ThemeLanguage, (o, v) => o.ThemeLanguage = v);

        /// <summary>
        /// Gets or sets the active density preset for <see cref="SynthoraTheme"/>.
        /// </summary>
        public ThemeDensity ThemeDensity
        {
            get => _themeDensity;
            set => SetAndRaise(ThemeDensityProperty, ref _themeDensity, value);
        }

        /// <summary>
        /// Gets or sets the active language resources for <see cref="SynthoraTheme"/>.
        /// </summary>
        public ThemeLanguage ThemeLanguage
        {
            get => _themeLanguage;
            set => SetAndRaise(ThemeLanguageProperty, ref _themeLanguage, value);
        }

        /// <summary>
        /// Gets the active <see cref="SynthoraTheme"/> instance from the current Avalonia application.
        /// </summary>
        public static SynthoraTheme? Current => Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault();

        /// <summary>
        /// Initializes a new instance of the <see cref="SynthoraTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SynthoraTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
            UpdateLanguageResource(ThemeLanguage);
            UpdateDensityResource(ThemeDensity);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == ThemeDensityProperty)
            {
                UpdateDensityResource(change.GetNewValue<ThemeDensity>());
            }
            else if (change.Property == ThemeLanguageProperty)
            {
                UpdateLanguageResource(change.GetNewValue<ThemeLanguage>());
            }
        }

        private void UpdateDensityResource(ThemeDensity density)
        {
            var resourceKey = density switch
            {
                ThemeDensity.Compact => DensityCompactResourceKey,
                ThemeDensity.Spacious => DensitySpaciousResourceKey,
                _ => DensityNormalResourceKey,
            };

            UpdateMergedResource(resourceKey, ref _currentDensityResource);
        }

        private void UpdateLanguageResource(ThemeLanguage language)
        {
            var resourceKey = language switch
            {
                ThemeLanguage.SimplifiedChinese => LanguageSimplifiedChineseResourceKey,
                ThemeLanguage.TraditionalChinese => LanguageTraditionalChineseResourceKey,
                ThemeLanguage.Korean => LanguageKoreanResourceKey,
                ThemeLanguage.Japanese => LanguageJapaneseResourceKey,
                _ => LanguageEnglishResourceKey,
            };

            UpdateMergedResource(resourceKey, ref _currentLanguageResource);
        }

        private void UpdateMergedResource(string resourceKey, ref IResourceProvider? currentResource)
        {
            if (TryGetResource(resourceKey, null, out var res) && res is IResourceProvider newResource)
            {
                if (ReferenceEquals(currentResource, newResource))
                {
                    return;
                }

                if (currentResource != null)
                {
                    Resources.MergedDictionaries.Remove(currentResource);
                }

                Resources.MergedDictionaries.Add(newResource);
                currentResource = newResource;
            }
            else
            {
                throw new Exception($"[SynthoraTheme] Error: Resource key '{resourceKey}' not found.");
            }
        }
    }
}