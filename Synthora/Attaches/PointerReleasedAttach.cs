using System.Windows.Input;
using Avalonia;
using Avalonia.Input;

namespace Synthora.Attaches
{
    public class PointerReleasedAttach
    {
        public static readonly AttachedProperty<string?> IgnoredSourceTypesProperty =
            AvaloniaProperty.RegisterAttached<PointerReleasedAttach, InputElement, string?>("IgnoredSourceTypes");

        public static readonly AttachedProperty<PointerButtons> ButtonsProperty =
            AvaloniaProperty.RegisterAttached<PointerReleasedAttach, InputElement, PointerButtons>("Buttons", PointerButtons.Left);

        public static readonly AttachedProperty<ICommand?> CommandProperty =
            AvaloniaProperty.RegisterAttached<PointerReleasedAttach, InputElement, ICommand?>("Command");

        public static readonly AttachedProperty<object?> CommandParameterProperty =
           AvaloniaProperty.RegisterAttached<PointerReleasedAttach, InputElement, object?>("CommandParameter");

        static PointerReleasedAttach()
        {
            CommandProperty.Changed.AddClassHandler<InputElement, ICommand?>((s, e) => OnCommandChanged(e));
        }

        public static string? GetIgnoredSourceTypes(InputElement element) => element.GetValue(IgnoredSourceTypesProperty);
        public static void SetIgnoredSourceTypes(InputElement element, string? value) => element.SetValue(IgnoredSourceTypesProperty, value);

        public static PointerButtons GetButtons(InputElement element) => element.GetValue(ButtonsProperty);
        public static void SetButtons(InputElement element, PointerButtons value) => element.SetValue(ButtonsProperty, value);

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

            inputElement.RemoveHandler(InputElement.PointerReleasedEvent, InputElement_PointerReleased);

            if (e.NewValue.Value != null)
            {
                inputElement.AddHandler(InputElement.PointerReleasedEvent, InputElement_PointerReleased, handledEventsToo: true);
            }
        }

        private static void InputElement_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (sender is InputElement inputElement)
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

                var command = GetCommand(inputElement);
                var commandParameter = GetCommandParameter(inputElement);
                var buttons = GetButtons(inputElement);
                if ((buttons & PointerButtons.Left) != 0 && e.InitialPressMouseButton == MouseButton.Left)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.Right) != 0 && e.InitialPressMouseButton == MouseButton.Right)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.Middle) != 0 && e.InitialPressMouseButton == MouseButton.Middle)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.XButton1) != 0 && e.InitialPressMouseButton == MouseButton.XButton1)
                {
                    command?.Execute(commandParameter);
                }
                if ((buttons & PointerButtons.XButton2) != 0 && e.InitialPressMouseButton == MouseButton.XButton2)
                {
                    command?.Execute(commandParameter);
                }
            }
        }
    }
}