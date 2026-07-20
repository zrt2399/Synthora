using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Controls.Primitives;
using Synthora.Input;

namespace Synthora.Utils
{
    internal class ScrollBarUtils
    {
        public static ICommand ScrollToHomeCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarScrollToHome);
        public static ICommand ScrollToEndCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarScrollToEnd);
        public static ICommand LargeDecrementCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarLargeDecrement);
        public static ICommand LargeIncrementCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarLargeIncrement);
        public static ICommand SmallDecrementCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarSmallDecrement);
        public static ICommand SmallIncrementCommand { get; } = new RelayCommand<ScrollBar?>(ScrollBarSmallIncrement);

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(LargeDecrement))]
        public static extern void LargeDecrement(ScrollBar scrollBar);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(LargeIncrement))]
        public static extern void LargeIncrement(ScrollBar scrollBar);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(SmallDecrement))]
        public static extern void SmallDecrement(ScrollBar scrollBar);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(SmallIncrement))]
        public static extern void SmallIncrement(ScrollBar scrollBar);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(OnScroll))]
        public static extern void OnScroll(ScrollBar scrollBar, ScrollEventType scrollEventType);

        public static void ScrollBarScrollToHome(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                scrollBar.SetCurrentValue(RangeBase.ValueProperty, scrollBar.Minimum);
                OnScroll(scrollBar, ScrollEventType.LargeDecrement);
            }
        }

        public static void ScrollBarScrollToEnd(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                scrollBar.SetCurrentValue(RangeBase.ValueProperty, scrollBar.Maximum);
                OnScroll(scrollBar, ScrollEventType.LargeIncrement);
            }
        }

        public static void ScrollBarLargeDecrement(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                LargeDecrement(scrollBar);
            }
        }

        public static void ScrollBarLargeIncrement(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                LargeIncrement(scrollBar);
            }
        }

        public static void ScrollBarSmallDecrement(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                SmallDecrement(scrollBar);
            }
        }

        public static void ScrollBarSmallIncrement(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                SmallIncrement(scrollBar);
            }
        }
    }
}