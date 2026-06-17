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
        /// Uses standard density resources.
        /// </summary>
        [Description("默认")]
        Standard,
        /// <summary>
        /// Uses comfortable density resources.
        /// </summary>
        [Description("宽松")]
        Comfortable
    }

    /// <summary>
    /// Provides the Synthora theme resources and control styles for an Avalonia application.
    /// </summary>
    public class SynthoraTheme : Styles
    {
        private ThemeDensity _themeDensity = ThemeDensity.Standard;
        private IResourceProvider? _currentDensityResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynthoraTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SynthoraTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
            UpdateDensityResource(ThemeDensity);
        }

        static SynthoraTheme()
        {
            ThemeDensityProperty.Changed.AddClassHandler<SynthoraTheme, ThemeDensity>((s, e) => s.OnThemeDensityChanged(e));
        }

        /// <summary>
        /// Defines the <see cref="ThemeDensity"/> property.
        /// </summary>
        public static readonly DirectProperty<SynthoraTheme, ThemeDensity> ThemeDensityProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, ThemeDensity>(
                nameof(ThemeDensity), o => o.ThemeDensity, (o, v) => o.ThemeDensity = v);

        /// <summary>
        /// Gets or sets the active density preset for <see cref="SynthoraTheme"/>.
        /// </summary>
        public ThemeDensity ThemeDensity
        {
            get => _themeDensity;
            set => SetAndRaise(ThemeDensityProperty, ref _themeDensity, value);
        }

        private void OnThemeDensityChanged(AvaloniaPropertyChangedEventArgs<ThemeDensity> e)
        {
            UpdateDensityResource(e.NewValue.Value);
        }

        private void UpdateDensityResource(ThemeDensity style)
        {
            string resourceKey = style switch
            {
                ThemeDensity.Compact => "DensityCompact",
                ThemeDensity.Comfortable => "DensityComfortable",
                _ => "DensityStandard",
            };

            if (TryGetResource(resourceKey, null, out var res) && res is IResourceProvider newRes)
            {
                if (ReferenceEquals(_currentDensityResource, newRes))
                {
                    return;
                }

                if (_currentDensityResource != null)
                {
                    Resources.MergedDictionaries.Remove(_currentDensityResource);
                }

                Resources.MergedDictionaries.Add(newRes);
                _currentDensityResource = newRes;
            }
            else
            {
                throw new Exception($"[SynthoraTheme] Error: Resource key '{resourceKey}' not found.");
            }
        }

        /// <summary>
        /// Sets the density preset on the <see cref="SynthoraTheme"/> in the current application.
        /// </summary>
        public static bool SetDensity(ThemeDensity newDensity)
        {
            if (Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault() is { } currentThemeInstance)
            {
                currentThemeInstance.ThemeDensity = newDensity;
                return currentThemeInstance.ThemeDensity == newDensity;
            }
            return false;
        }

        /// <summary>
        /// Gets the density preset from the <see cref="SynthoraTheme"/> in the current application.
        /// </summary>
        public static ThemeDensity GetCurrentDensity()
        {
            return Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault()?.ThemeDensity ?? ThemeDensity.Standard;
        }
    }
}