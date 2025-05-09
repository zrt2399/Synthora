using System.Reflection;
using System.Windows.Input;
using Avalonia.Controls.Primitives;
using Synthora.Commands;

namespace Synthora.Utils
{
    internal class ScrollBarUtils
    {
        public static ICommand ScrollToHome { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToHome);
        public static ICommand ScrollToEnd { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToEnd);
        public static ICommand LargeDecrement { get; } = new RelayCommand<ScrollBar>(ScrollBarLargeDecrement);
        public static ICommand LargeIncrement { get; } = new RelayCommand<ScrollBar>(ScrollBarLargeIncrement);
        public static ICommand SmallDecrement { get; } = new RelayCommand<ScrollBar>(ScrollBarSmallDecrement);
        public static ICommand SmallIncrement { get; } = new RelayCommand<ScrollBar>(ScrollBarSmallIncrement);

        private static void InvokeMethod(ScrollBar? scrollBar, string methodName)
        {
            scrollBar?.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(scrollBar, null);
        }

        public static void ScrollBarScrollToHome(ScrollBar? scrollBar)
        {
            scrollBar?.SetCurrentValue(RangeBase.ValueProperty, 0);
            InvokeMethod(scrollBar, "LargeDecrement");
        }

        public static void ScrollBarScrollToEnd(ScrollBar? scrollBar)
        {
            scrollBar?.SetCurrentValue(RangeBase.ValueProperty, double.MaxValue);
            InvokeMethod(scrollBar, "LargeIncrement");
        }

        public static void ScrollBarLargeDecrement(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "LargeDecrement");
        }

        public static void ScrollBarLargeIncrement(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "LargeIncrement");
        }

        public static void ScrollBarSmallDecrement(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "SmallDecrement");
        }

        public static void ScrollBarSmallIncrement(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "SmallIncrement");
        }
    }
}