using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class OverviewViewModel : TreeMenuDemoItem
    {
        public OverviewViewModel()
        {
            IconKind = MaterialIconKind.ViewDashboard;
            SelectedThemeLanguage = SynthoraTheme.Current?.ThemeLanguage.Name ?? "en-US";
        }

        [ObservableProperty]
        public partial string SelectedThemeLanguage { get; set; }
        partial void OnSelectedThemeLanguageChanged(string value) => SynthoraTheme.Current?.ThemeLanguage = CultureInfo.GetCultureInfo(value);

        public void SetThemeLanguage(object? content)
        {
            if (content is string language)
            {
                SelectedThemeLanguage = language;
            }
        }
    }
}