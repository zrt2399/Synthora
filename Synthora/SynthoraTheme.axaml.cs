using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Provides the Synthora theme resources and control styles for an Avalonia application.
    /// </summary>
    public class SynthoraTheme : Styles
    {
        private static readonly CultureInfo _defaultThemeLanguage = CultureInfo.GetCultureInfo("en-US");

        internal const string DensityCompactResourceKey = "DensityCompact";
        internal const string DensitySpaciousResourceKey = "DensitySpacious";
        // Register each culture resource here and in SynthoraTheme.axaml.
        private static readonly Dictionary<string, string> LanguageResourceKeys = new(StringComparer.OrdinalIgnoreCase)
        {
            ["zh-Hans"] = "LanguageZh_Hans",
            ["zh-Hant"] = "LanguageZh_Hant",
            ["ja-JP"] = "LanguageJa_JP",
            ["ko-KR"] = "LanguageKo_KR",
            ["es-ES"] = "LanguageEs_ES",
        };

        private ThemeDensity _themeDensity = ThemeDensity.Normal;
        private CultureInfo _themeLanguage = _defaultThemeLanguage;
        private IResourceProvider? _currentDensityResource;
        private readonly List<IResourceProvider> _currentLanguageResources = [];

        /// <summary>
        /// Defines the <see cref="ThemeDensity"/> property.
        /// </summary>
        public static readonly DirectProperty<SynthoraTheme, ThemeDensity> ThemeDensityProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, ThemeDensity>(
                nameof(ThemeDensity), o => o.ThemeDensity, (o, v) => o.ThemeDensity = v);

        /// <summary>
        /// Defines the <see cref="ThemeLanguage"/> property.
        /// </summary>
        public static readonly DirectProperty<SynthoraTheme, CultureInfo> ThemeLanguageProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, CultureInfo>(
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
        public CultureInfo ThemeLanguage
        {
            get => _themeLanguage;
            set => SetAndRaise(ThemeLanguageProperty, ref _themeLanguage, value ?? _defaultThemeLanguage);
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
                UpdateLanguageResource(change.GetNewValue<CultureInfo>());
            }
        }

        private void UpdateDensityResource(ThemeDensity density)
        {
            var resourceKey = density switch
            {
                ThemeDensity.Compact => DensityCompactResourceKey,
                ThemeDensity.Spacious => DensitySpaciousResourceKey,
                _ => null,
            };

            if (resourceKey == null)
            {
                if (_currentDensityResource != null)
                {
                    Resources.MergedDictionaries.Remove(_currentDensityResource);
                    _currentDensityResource = null;
                }

                return;
            }

            UpdateMergedResource(resourceKey, ref _currentDensityResource);
        }

        private void UpdateLanguageResource(CultureInfo language)
        {
            var resourceKeys = new List<string>();
            for (var culture = language; !string.IsNullOrEmpty(culture.Name); culture = culture.Parent)
            {
                if (LanguageResourceKeys.TryGetValue(culture.Name, out var key))
                {
                    resourceKeys.Add(key);
                }
            }

            resourceKeys.Reverse();

            var resources = new List<IResourceProvider>();
            foreach (var key in resourceKeys)
            {
                if (!TryGetResource(key, null, out var resource) || resource is not IResourceProvider provider)
                {
                    throw new Exception($"[SynthoraTheme] Error: Resource key '{key}' not found.");
                }

                resources.Add(provider);
            }

            foreach (var resource in _currentLanguageResources)
            {
                Resources.MergedDictionaries.Remove(resource);
            }

            _currentLanguageResources.Clear();
            foreach (var resource in resources)
            {
                Resources.MergedDictionaries.Add(resource);
                _currentLanguageResources.Add(resource);
            }
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