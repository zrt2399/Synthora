using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Synthora.Attaches
{
    public class ComboBoxAttach
    {
        public static readonly AttachedProperty<CornerRadius> PopupCornerRadiusProperty =
            AvaloniaProperty.RegisterAttached<ComboBoxAttach, Control, CornerRadius>("PopupCornerRadius");

        public static readonly AttachedProperty<Thickness> PopupBorderThicknessProperty =
            AvaloniaProperty.RegisterAttached<ComboBoxAttach, Control, Thickness>("PopupBorderThickness");

        public static readonly AttachedProperty<Thickness> PopupPaddingProperty =
            AvaloniaProperty.RegisterAttached<ComboBoxAttach, Control, Thickness>("PopupPadding");

        public static readonly AttachedProperty<bool> UsePopupPaddingForBringIntoViewProperty =
            AvaloniaProperty.RegisterAttached<ComboBoxAttach, Control, bool>("UsePopupPaddingForBringIntoView");

        static ComboBoxAttach()
        {
            UsePopupPaddingForBringIntoViewProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnUsePopupPaddingForBringIntoViewChanged(e));
        }

        public static CornerRadius GetPopupCornerRadius(Control obj) => obj.GetValue(PopupCornerRadiusProperty);
        public static void SetPopupCornerRadius(Control obj, CornerRadius value) => obj.SetValue(PopupCornerRadiusProperty, value);

        public static Thickness GetPopupBorderThickness(Control obj) => obj.GetValue(PopupBorderThicknessProperty);
        public static void SetPopupBorderThickness(Control obj, Thickness value) => obj.SetValue(PopupBorderThicknessProperty, value);

        public static Thickness GetPopupPadding(Control obj) => obj.GetValue(PopupPaddingProperty);
        public static void SetPopupPadding(Control obj, Thickness value) => obj.SetValue(PopupPaddingProperty, value);

        public static bool GetUsePopupPaddingForBringIntoView(Control obj) => obj.GetValue(UsePopupPaddingForBringIntoViewProperty);
        public static void SetUsePopupPaddingForBringIntoView(Control obj, bool value) => obj.SetValue(UsePopupPaddingForBringIntoViewProperty, value);

        private static void OnUsePopupPaddingForBringIntoViewChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is ComboBoxItem comboBoxItem)
            {
                comboBoxItem.RemoveHandler(Control.RequestBringIntoViewEvent, OnRequestBringIntoView);
                if (e.NewValue.Value)
                {
                    comboBoxItem.AddHandler(Control.RequestBringIntoViewEvent, OnRequestBringIntoView, handledEventsToo: true);
                }
            }
        }

        private static void OnRequestBringIntoView(object? sender, RequestBringIntoViewEventArgs e)
        {
            if (sender is ComboBoxItem comboBoxItem && comboBoxItem.Parent is ComboBox comboBox)
            {
                var margin = comboBox.Presenter?.Margin ?? default;
                if (margin != default)
                {
                    var left = Math.Max(0, margin.Left);
                    var top = Math.Max(0, margin.Top);
                    var right = Math.Max(0, margin.Right);
                    var bottom = Math.Max(0, margin.Bottom);

                    e.TargetRect = new Rect(
                        -left,
                        -top,
                        comboBoxItem.Bounds.Width + left + right,
                        comboBoxItem.Bounds.Height + top + bottom);
                }
            }
        }
    }
}