using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Synthora.Messaging;

namespace Synthora.Controls
{
    public class DialogClosedEventArgs(DialogResult dialogResult) : EventArgs
    {
        public DialogResult DialogResult { get; } = dialogResult;
    }

    public class AlertDialogHost : DropShadowChrome
    {
        public static readonly StyledProperty<double> BlurRadiusProperty =
            AvaloniaProperty.Register<AlertDialogHost, double>(nameof(BlurRadius), 1d);

        public static readonly StyledProperty<object?> ParentContentProperty =
            AvaloniaProperty.Register<AlertDialogHost, object?>(nameof(ParentContent));

        public static readonly StyledProperty<string?> TitleProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Title));

        public static readonly StyledProperty<DialogButton> DialogButtonProperty =
            AvaloniaProperty.Register<AlertDialogHost, DialogButton>(nameof(DialogButton));

        public static readonly StyledProperty<IconType> IconTypeProperty =
            AvaloniaProperty.Register<AlertDialogHost, IconType>(nameof(IconType));

        public event EventHandler<DialogClosedEventArgs>? DialogClosed;

        public double BlurRadius
        {
            get => GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        public object? ParentContent
        {
            get => GetValue(ParentContentProperty);
            set => SetValue(ParentContentProperty, value);
        }

        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public DialogButton DialogButton
        {
            get => GetValue(DialogButtonProperty);
            set => SetValue(DialogButtonProperty, value);
        }

        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var border = e.NameScope.Find<Border>("PART_DialogBorder");
            if (border != null)
            {
                border.Opacity = 0;
                border.Opacity = 1;
            }
        }

        public void OK() => DialogClosed?.Invoke(this, new DialogClosedEventArgs(DialogResult.OK));
        public void Cancel() => DialogClosed?.Invoke(this, new DialogClosedEventArgs(DialogResult.Cancel));
        public void Yes() => DialogClosed?.Invoke(this, new DialogClosedEventArgs(DialogResult.Yes));
        public void No() => DialogClosed?.Invoke(this, new DialogClosedEventArgs(DialogResult.No));
        public void Abort() => DialogClosed?.Invoke(this, new DialogClosedEventArgs(DialogResult.Abort));
    }
}