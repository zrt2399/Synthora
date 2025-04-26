using System.Reflection;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Synthora.Commands;

namespace Synthora.Utils
{
    internal class ScrollBarUtils
    {
        public static ICommand ScrollToTop { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToHome);
        public static ICommand ScrollToBottom { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToEnd);
        public static ICommand PageUp { get; } = new RelayCommand<ScrollBar>(ScrollBarPageUp);
        public static ICommand PageDown { get; } = new RelayCommand<ScrollBar>(ScrollBarPageDown);
        public static ICommand LineUp { get; } = new RelayCommand<ScrollBar>(ScrollBarLineUp);
        public static ICommand LineDown { get; } = new RelayCommand<ScrollBar>(ScrollBarLineDown);

        public static ICommand ScrollToLeftEdge { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToHome);
        public static ICommand ScrollToRightEdge { get; } = new RelayCommand<ScrollBar>(ScrollBarScrollToEnd);
        public static ICommand PageLeft { get; } = new RelayCommand<ScrollBar>(ScrollBarPageLeft);
        public static ICommand PageRight { get; } = new RelayCommand<ScrollBar>(ScrollBarPageRight);
        public static ICommand LineLeft { get; } = new RelayCommand<ScrollBar>(ScrollBarLineLeft);
        public static ICommand LineRight { get; } = new RelayCommand<ScrollBar>(ScrollBarLineRight);

        private static void InvokeMethod(ScrollBar? scrollBar, string methodName)
        {
            scrollBar?.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(scrollBar, null);
        }

        private static ScrollViewer? GetOwnerScrollViewer(ScrollBar? scrollBar)
        {
            return scrollBar?.GetType().GetField("_owner", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(scrollBar) as ScrollViewer;
        }

        public static void ScrollBarScrollToHome(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToHome();
                    return;
                }
                scrollBar.Value = 0;
                InvokeMethod(scrollBar, "LargeDecrement");
            }
        }

        public static void ScrollBarScrollToEnd(ScrollBar? scrollBar)
        {
            if (scrollBar != null)
            {
                if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToEnd();
                    return;
                }
                scrollBar.Value = double.MaxValue;
                InvokeMethod(scrollBar, "LargeIncrement");
            }
        }

        public static void ScrollBarPageUp(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.PageUp();
                return;
            }
            InvokeMethod(scrollBar, "LargeDecrement");
        }

        public static void ScrollBarPageDown(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.PageDown();
                return;
            }
            InvokeMethod(scrollBar, "LargeIncrement");
        }

        public static void ScrollBarLineUp(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.LineUp();
                return;
            }
            InvokeMethod(scrollBar, "SmallDecrement");
        }

        public static void ScrollBarLineDown(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.LineDown();
                return;
            }
            InvokeMethod(scrollBar, "SmallIncrement");
        }

        public static void ScrollBarPageLeft(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.PageLeft();
                return;
            }
            InvokeMethod(scrollBar, "LargeDecrement");
        }

        public static void ScrollBarPageRight(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.PageRight();
                return;
            }
            InvokeMethod(scrollBar, "LargeIncrement");
        }

        public static void ScrollBarLineLeft(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.LineLeft();
                return;
            }
            InvokeMethod(scrollBar, "SmallDecrement");
        }

        public static void ScrollBarLineRight(ScrollBar? scrollBar)
        {
            if (GetOwnerScrollViewer(scrollBar) is ScrollViewer scrollViewer)
            {
                scrollViewer.LineRight();
                return;
            }
            InvokeMethod(scrollBar, "SmallIncrement");
        }
    }
}