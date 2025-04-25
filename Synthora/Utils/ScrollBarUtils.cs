using System.Reflection;
using System.Windows.Input;
using Avalonia.Controls.Primitives;
using Synthora.Commands;

namespace Synthora.Utils
{
    internal class ScrollBarUtils
    {
        public static ICommand ScrollToTop { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToTop);
        public static ICommand ScrollToBottom { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToBottom);
        public static ICommand PageUp { get; } = new RelayCommand<ScrollBar>(ScrollBarPageUp);
        public static ICommand PageDown { get; } = new RelayCommand<ScrollBar>(ScrollBarPageDown);
        public static ICommand LineUp { get; } = new RelayCommand<ScrollBar>(ScrollBarLineUp);
        public static ICommand LineDown { get; } = new RelayCommand<ScrollBar>(ScrollBarLineDown);
        public static ICommand ScrollToLeftEdge { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToTop);
        public static ICommand ScrollToRightEdge { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToBottom);
        public static ICommand PageLeft { get; } = new RelayCommand<ScrollBar>(ScrollBarPageUp);
        public static ICommand PageRight { get; } = new RelayCommand<ScrollBar>(ScrollBarPageDown);
        public static ICommand LineLeft { get; } = new RelayCommand<ScrollBar>(ScrollBarLineUp);
        public static ICommand LineRight { get; } = new RelayCommand<ScrollBar>(ScrollBarLineDown);

        private static void InvokeMethod(ScrollBar? scrollBar, string name)
        {
            scrollBar?.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(scrollBar, null);
        }

        public static void ScrollBarScrollToTop(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                scrollBar.Value = 0;
                InvokeMethod(scrollBar, "LargeDecrement");
            }
        }

        public static void ScrollBarScrollToBottom(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                scrollBar.Value = double.MaxValue;
                InvokeMethod(scrollBar, "LargeIncrement");
            }
        }

        public static void ScrollBarPageUp(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "LargeDecrement");
        }

        public static void ScrollBarPageDown(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "LargeIncrement");
        }

        public static void ScrollBarLineUp(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "SmallDecrement");
        }

        public static void ScrollBarLineDown(ScrollBar? scrollBar)
        {
            InvokeMethod(scrollBar, "SmallIncrement");
        }
    }
}