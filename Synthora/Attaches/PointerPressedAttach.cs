using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Input;

namespace Synthora.Attaches
{
    [Flags]
    public enum PointerButtons
    {
        None = 0,
        Left = 1,
        Middle = 1 << 1,
        Right = 1 << 2,
        XButton1 = 1 << 3,
        XButton2 = 1 << 4,
        BarrelButton = 1 << 5
    }

    public class PointerPressedAttach
    {
        public static readonly AttachedProperty<string?> IgnoredSourceTypesProperty =
            AvaloniaProperty.RegisterAttached<PointerPressedAttach, InputElement, string?>("IgnoredSourceTypes");

        public static readonly AttachedProperty<PointerButtons> ButtonsProperty =
            AvaloniaProperty.RegisterAttached<PointerPressedAttach, InputElement, PointerButtons>("Buttons", PointerButtons.Left);

        public static readonly AttachedProperty<int> ClickCountProperty =
            AvaloniaProperty.RegisterAttached<PointerPressedAttach, InputElement, int>("ClickCount", 1);

        public static readonly AttachedProperty<ICommand?> CommandProperty =
            AvaloniaProperty.RegisterAttached<PointerPressedAttach, InputElement, ICommand?>("Command");

        public static readonly AttachedProperty<object?> CommandParameterProperty =
           AvaloniaProperty.RegisterAttached<PointerPressedAttach, InputElement, object?>("CommandParameter");

        static PointerPressedAttach()
        {
            CommandProperty.Changed.AddClassHandler<InputElement, ICommand?>((s, e) => OnCommandChanged(e));
        }

        public static string? GetIgnoredSourceTypes(InputElement element) => element.GetValue(IgnoredSourceTypesProperty);
        public static void SetIgnoredSourceTypes(InputElement element, string? value) => element.SetValue(IgnoredSourceTypesProperty, value);

        public static PointerButtons GetButtons(InputElement element) => element.GetValue(ButtonsProperty);
        public static void SetButtons(InputElement element, PointerButtons value) => element.SetValue(ButtonsProperty, value);

        public static int GetClickCount(InputElement element) => element.GetValue(ClickCountProperty);
        public static void SetClickCount(InputElement element, int value) => element.SetValue(ClickCountProperty, value);

        public static ICommand? GetCommand(InputElement element) => element.GetValue(CommandProperty);
        public static void SetCommand(InputElement element, ICommand? value) => element.SetValue(CommandProperty, value);

        public static object? GetCommandParameter(InputElement element) => element.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(InputElement element, object? value) => element.SetValue(CommandParameterProperty, value);

        private static void OnCommandChanged(AvaloniaPropertyChangedEventArgs<ICommand?> e)
        {
            if (e.Sender is not InputElement inputElement)
            {
                return;
            }

            inputElement.RemoveHandler(InputElement.PointerPressedEvent, InputElement_PointerPressed);

            if (e.NewValue.Value != null)
            {
                inputElement.AddHandler(InputElement.PointerPressedEvent, InputElement_PointerPressed, handledEventsToo: true);
            }
        }

        private static void InputElement_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (sender is InputElement inputElement && e.ClickCount == GetClickCount(inputElement))
            {
                var ignoredSourceTypes = GetIgnoredSourceTypes(inputElement);
                if (!string.IsNullOrEmpty(ignoredSourceTypes))
                {
                    var types = ignoredSourceTypes.Split(',', '|');
                    foreach (var item in types)
                    {
                        if (e.Source?.GetType().Name == item)
                        {
                            return;
                        }
                    }
                }

                var pointerPoint = e.GetCurrentPoint(inputElement).Properties;

                var command = GetCommand(inputElement);
                var commandParameter = GetCommandParameter(inputElement);
                var buttons = GetButtons(inputElement);
                if ((buttons & PointerButtons.Left) != 0 && pointerPoint.IsLeftButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.Right) != 0 && pointerPoint.IsRightButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.Middle) != 0 && pointerPoint.IsMiddleButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.XButton1) != 0 && pointerPoint.IsXButton1Pressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.XButton2) != 0 && pointerPoint.IsXButton2Pressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.BarrelButton) != 0 && pointerPoint.IsBarrelButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
            }
        }
    }
}