using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Synthora.Converters;

namespace Synthora.Attaches
{
    public class CornerRadiusAttach
    {
        public static readonly AttachedProperty<bool> IsCircularProperty =
            AvaloniaProperty.RegisterAttached<CornerRadiusAttach, InputElement, bool>("IsCircular");

        static CornerRadiusAttach()
        {
            IsCircularProperty.Changed.AddClassHandler<InputElement, bool>((s, e) => OnIsCircularChanged(e));
        }

        public static bool GetIsCircular(InputElement element) => element.GetValue(IsCircularProperty);
        public static void SetIsCircular(InputElement element, bool value) => element.SetValue(IsCircularProperty, value);

        private static void OnIsCircularChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is InputElement element && GetCornerRadiusProperty(element) is { } avaloniaProperty)
            {
                if (e.NewValue.Value)
                {
                    var multiBinding = new MultiBinding
                    {
                        Converter = BorderCircularConverter.Instance
                    };

                    multiBinding.Bindings.Add(CompiledBinding.Create<InputElement, double>(x => x.Bounds.Width, element));
                    multiBinding.Bindings.Add(CompiledBinding.Create<InputElement, double>(x => x.Bounds.Height, element));

                    element.Bind(avaloniaProperty, multiBinding);
                }
                else
                {
                    element.ClearValue(avaloniaProperty);
                }
            }
        }

        private static AvaloniaProperty<CornerRadius>? GetCornerRadiusProperty(InputElement element)
        {
            return element switch
            {
                Border => Border.CornerRadiusProperty,
                TemplatedControl => TemplatedControl.CornerRadiusProperty,
                _ => null
            };
        }
    }
}