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

        public static void SetIsCircular(InputElement element, bool value) => element.SetValue(IsCircularProperty, value);
        public static bool GetIsCircular(InputElement element) => element.GetValue(IsCircularProperty);

        private static void OnIsCircularChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is InputElement element && GetCornerRadiusProperty(element) is AvaloniaProperty avaloniaProperty)
            {
                if (e.NewValue.Value)
                {
                    MultiBinding multiBinding = new MultiBinding
                    {
                        Converter = BorderCircularConverter.Instance
                    };
                    multiBinding.Bindings.Add(new Binding("Bounds.Width")
                    {
                        Source = element
                    });
                    multiBinding.Bindings.Add(new Binding("Bounds.Height")
                    {
                        Source = element
                    });
                    element.Bind(avaloniaProperty, multiBinding);
                }
                else
                {
                    //element.ClearValue(new AvaloniaProperty<double>("Bounds.Width"), );
                    //element.ClearValue("Bounds.Height");
                    element.ClearValue(avaloniaProperty);
                }
            }
        }

        private static AvaloniaProperty? GetCornerRadiusProperty(InputElement element)
        {
            if (element is Border)
            {
                return Border.CornerRadiusProperty;
            }
            else if (element is TemplatedControl)
            {
                return TemplatedControl.CornerRadiusProperty;
            }
            return null;
        }
    }
}