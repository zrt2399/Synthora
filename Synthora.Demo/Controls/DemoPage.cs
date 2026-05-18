using Avalonia;
using Avalonia.Controls;
using Material.Icons;

namespace Synthora.Demo.Controls
{
    public class DemoPage : ContentControl
    {
        public static readonly StyledProperty<string?> TitleProperty =
            AvaloniaProperty.Register<DemoPage, string?>(nameof(Title));

        public static readonly StyledProperty<string?> DescriptionProperty =
            AvaloniaProperty.Register<DemoPage, string?>(nameof(Description));

        public static readonly StyledProperty<MaterialIconKind> IconKindProperty =
            AvaloniaProperty.Register<DemoPage, MaterialIconKind>(nameof(IconKind));

        public static readonly StyledProperty<bool> IsContentEnabledProperty =
            AvaloniaProperty.Register<DemoPage, bool>(nameof(IsContentEnabled), true);

        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string? Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public MaterialIconKind IconKind
        {
            get => GetValue(IconKindProperty);
            set => SetValue(IconKindProperty, value);
        }

        public bool IsContentEnabled
        {
            get => GetValue(IsContentEnabledProperty);
            set => SetValue(IsContentEnabledProperty, value);
        }
    }
}