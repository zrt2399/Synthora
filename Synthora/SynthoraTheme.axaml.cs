using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Synthora
{
    public enum ThemeMode
    {
        [Description("系统")]
        Default,
        [Description("浅色")]
        Light,
        [Description("深色")]
        Dark
    }

    public enum DensityStyle
    {
        [Description("紧凑")]
        Compact,
        [Description("正常")]
        Normal
    }

    public class SynthoraTheme : Styles
    {
        private DensityStyle _densityStyle;
        private IResourceProvider? _currentDensityResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynthoraTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SynthoraTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
            UpdateDensityResource(DensityStyle);
        }

        static SynthoraTheme()
        {
            DensityStyleProperty.Changed.AddClassHandler<SynthoraTheme, DensityStyle>((s, e) => s.OnDensityStyleChanged(e));
        }

        public static readonly DirectProperty<SynthoraTheme, DensityStyle> DensityStyleProperty =
            AvaloniaProperty.RegisterDirect<SynthoraTheme, DensityStyle>(
                nameof(DensityStyle), o => o.DensityStyle, (o, v) => o.DensityStyle = v);

        public DensityStyle DensityStyle
        {
            get => _densityStyle;
            set => SetAndRaise(DensityStyleProperty, ref _densityStyle, value);
        }

        private void OnDensityStyleChanged(AvaloniaPropertyChangedEventArgs<DensityStyle> e)
        {
            UpdateDensityResource(e.NewValue.Value);
        }

        private void UpdateDensityResource(DensityStyle style)
        {
            string resourceKey = style == DensityStyle.Compact ? "DensityCompact" : "DensityNormal";

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

        public static bool SetDensity(DensityStyle newDensity)
        {
            if (Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault() is { } currentThemeInstance)
            {
                currentThemeInstance.DensityStyle = newDensity;
                return currentThemeInstance.DensityStyle == newDensity;
            }
            return false;
        }

        public static DensityStyle GetCurrentDensity()
        {
            return Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault()?.DensityStyle ?? DensityStyle.Normal;
        }
    }
}