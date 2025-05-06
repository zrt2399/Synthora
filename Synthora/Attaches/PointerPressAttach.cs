using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Input;

namespace Synthora.Attaches
{
    [Flags]
    public enum PointerPressMode
    {
        None = 0,
        Left = 1,
        Middle = 1 << 1,
        Right = 1 << 2,
        XButton1 = 1 << 3,
        XButton2 = 1 << 4,
        BarrelButton = 1 << 5
    }

    public class PointerPressAttach
    {
        static PointerPressAttach()
        {
            CommandProperty.Changed.AddClassHandler<InputElement, ICommand?>((s, e) => OnCommandChanged(e));
        }

        public static readonly AttachedProperty<string?> IgnoreElementProperty =
            AvaloniaProperty.RegisterAttached<PointerPressAttach, InputElement, string?>("IgnoreElement");

        public static string? GetIgnoreElement(InputElement element)
        {
            return element.GetValue(IgnoreElementProperty);
        }

        public static void SetIgnoreElement(InputElement element, string? value)
        {
            element.SetValue(IgnoreElementProperty, value);
        }

        public static readonly AttachedProperty<PointerPressMode> PointerPressModeProperty =
            AvaloniaProperty.RegisterAttached<PointerPressAttach, InputElement, PointerPressMode>("PointerPressMode", PointerPressMode.Left | PointerPressMode.Right);

        public static PointerPressMode GetPointerPressMode(InputElement element)
        {
            return element.GetValue(PointerPressModeProperty);
        }

        public static void SetPointerPressMode(InputElement element, PointerPressMode value)
        {
            element.SetValue(PointerPressModeProperty, value);
        }

        public static readonly AttachedProperty<int> PointerPressCountProperty =
            AvaloniaProperty.RegisterAttached<PointerPressAttach, InputElement, int>("PointerPressCount", 1);

        public static int GetPointerPressCount(InputElement element)
        {
            return element.GetValue(PointerPressCountProperty);
        }

        public static void SetPointerPressCount(InputElement element, int value)
        {
            element.SetValue(PointerPressCountProperty, value);
        }

        public static readonly AttachedProperty<ICommand?> CommandProperty =
            AvaloniaProperty.RegisterAttached<PointerPressAttach, InputElement, ICommand?>("Command");

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
            if (sender is InputElement inputElement && e.ClickCount == GetPointerPressCount(inputElement))
            {
                var ignoreElement = GetIgnoreElement(inputElement);
                if (!string.IsNullOrEmpty(ignoreElement))
                {
                    var types = ignoreElement.Split(',', '|');
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
                var PointerPressMode = GetPointerPressMode(inputElement);
                if ((PointerPressMode & PointerPressMode.Left) != 0 && pointerPoint.IsLeftButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((PointerPressMode & PointerPressMode.Right) != 0 && pointerPoint.IsRightButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((PointerPressMode & PointerPressMode.Middle) != 0 && pointerPoint.IsMiddleButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((PointerPressMode & PointerPressMode.XButton1) != 0 && pointerPoint.IsXButton1Pressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((PointerPressMode & PointerPressMode.XButton2) != 0 && pointerPoint.IsXButton2Pressed)
                {
                    command?.Execute(commandParameter);
                }
                if ((PointerPressMode & PointerPressMode.BarrelButton) != 0 && pointerPoint.IsBarrelButtonPressed)
                {
                    command?.Execute(commandParameter);
                }
            }
        }

        public static ICommand? GetCommand(InputElement element)
        {
            return element.GetValue(CommandProperty);
        }

        public static void SetCommand(InputElement element, ICommand? value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static readonly AttachedProperty<object?> CommandParameterProperty =
           AvaloniaProperty.RegisterAttached<PointerPressAttach, InputElement, object?>("CommandParameter");

        public static object? GetCommandParameter(InputElement element)
        {
            return element.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(InputElement element, object? value)
        {
            element.SetValue(CommandParameterProperty, value);
        }
    }
}