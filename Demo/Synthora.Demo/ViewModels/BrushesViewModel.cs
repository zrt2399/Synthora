using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class BrushesViewModel : TreeMenuDemoItem
    {
        [ObservableProperty]
        public partial ObservableCollection<string>? BrushKeys { get; set; }

        public BrushesViewModel()
        {
            IconKind = MaterialIconKind.BrushVariant;
            if (Application.Current is { } application)
            {
                application.ActualThemeVariantChanged += Current_ActualThemeVariantChanged;
            }
            InitializeBrushResources();
        }

        private void InitializeBrushResources()
        {
            if (Application.Current?.Styles.OfType<SynthoraTheme>().LastOrDefault() is not { } synthoraTheme)
            {
                return;
            }

            List<string> keys = [];
            keys.AddRange(synthoraTheme.Resources.Select(x => x.Key.ToString()!).Where(IsBrush));
            if (synthoraTheme.Resources.ThemeDictionaries[ThemeVariant.Default] is ResourceDictionary dictionary)
            {
                keys.AddRange(dictionary.Keys.Select(x => x.ToString()!).Where(IsBrush));
            }

            BrushKeys = new ObservableCollection<string>(keys);
        }

        private static bool IsBrush(string? key)
        {
            return !string.IsNullOrEmpty(key) && (key.Contains("Brush") || key.Contains("Background") || key.Contains("Foreground")) && !key.Contains("Color");
        }

        private void Current_ActualThemeVariantChanged(object? sender, EventArgs e)
        {
            var temp = BrushKeys;
            if (temp != null)
            {
                BrushKeys = new ObservableCollection<string>(temp);
            }
        }
    }
}