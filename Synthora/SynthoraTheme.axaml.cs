using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Synthora
{
    public enum DensityStyle
    {
        Compact,
        Normal
    }

    public class SynthoraTheme : Styles
    {
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
                nameof(DensityStyle),
                o => o.DensityStyle,
                (o, v) => o.DensityStyle = v);

        public DensityStyle DensityStyle
        {
            get;
            set => SetAndRaise(DensityStyleProperty, ref field, value);
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
                if (_currentDensityResource != null)
                {
                    Resources.MergedDictionaries.Remove(_currentDensityResource);
                }

                Resources.MergedDictionaries.Add(newRes);
                _currentDensityResource = newRes;
            }
            else
            {
                Debug.WriteLine($"[SynthoraTheme] Warning: Resource key '{resourceKey}' not found.");
            }
        }

        public static void SetDensity(DensityStyle newDensity)
        {
            if (Application.Current?.Styles.OfType<SynthoraTheme>().FirstOrDefault() is { } currentThemeInstance)
            {
                currentThemeInstance.DensityStyle = newDensity;
            }
        }

        public static DensityStyle GetCurrentDensity()
        {
            return Application.Current?.Styles.OfType<SynthoraTheme>().FirstOrDefault()?.DensityStyle ?? DensityStyle.Normal;
        }
    }
}