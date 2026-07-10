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
            SelectedThemeLanguage = SynthoraTheme.Current?.ThemeLanguage ?? ThemeLanguage.English;
        }

        [ObservableProperty]
        public partial ThemeLanguage SelectedThemeLanguage { get; set; }
        partial void OnSelectedThemeLanguageChanged(ThemeLanguage value) => SynthoraTheme.Current?.ThemeLanguage = value;

        public void SetThemeLanguage(object? content)
        {
            if (content is ThemeLanguage language)
            {
                SelectedThemeLanguage = language;
            }
        }
    }
}