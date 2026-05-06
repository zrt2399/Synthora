using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Synthora.Controls.DragTabControl.Panels;

public sealed class TopPanel : Panel
{
    private Parts? _parts;

    private Parts GetParts()
    {
        if (_parts.HasValue)
            return _parts.Value;

        _parts = new Parts
        {
            LeftContentControl = Children.Single(a => a.Name == "PART_LeftContent"),
            LeftDragWindowThumb = Children.Single(a => a.Name == "PART_LeftDragWindowThumb"),
            DragTabControl = Children.Single(a => a.Name == "PART_ItemsPresenter"),
            AddTabButton = Children.Single(a => a.Name == "PART_AddItemButton"),
            RightDragWindowThumb = Children.Single(a => a.Name == "PART_RightDragWindowThumb"),
            RightContentControl = Children.Single(a => a.Name == "PART_RightContent"),
        };

        return _parts.Value;
    }


    protected override Size MeasureOverride(Size availableSize)
    {
        double height = 0;
        double width = 0;

        if (Children.Count == 0)
            return new Size(width, height);

        double availableWidth = availableSize.Width;
        double availableHeight = availableSize.Height;

        var parts = GetParts();

        MeasureControl(parts.LeftContentControl, ref width, ref availableWidth, availableHeight);
        MeasureControl(parts.LeftDragWindowThumb, ref width, ref availableWidth, availableHeight);
        MeasureControl(parts.AddTabButton, ref width, ref availableWidth, availableHeight);
        MeasureControl(parts.RightDragWindowThumb, ref width, ref availableWidth, availableHeight);
        MeasureControl(parts.RightContentControl, ref width, ref availableWidth, availableHeight);

        parts.DragTabControl.Measure(new Size(availableWidth, availableHeight));
        width += parts.DragTabControl.DesiredSize.Width;
        Control[] affectingHeight =
            [parts.LeftContentControl, parts.DragTabControl, parts.AddTabButton, parts.RightContentControl];
        height = affectingHeight.Max(a => a.DesiredSize.Height);

        return new Size(width, height);

        static void MeasureControl(Control control, ref double w, ref double aW, in double h)
        {
            control.Measure(new Size(aW, h));
            w += control.DesiredSize.Width;
            aW -= control.DesiredSize.Width;
        }
    }


    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0)
            return finalSize;

        var parts = GetParts();
        var partsWidth = new PartsWidth
        {
            LeftContentControl = parts.LeftContentControl.DesiredSize.Width,
            LeftDragWindowThumb = parts.LeftDragWindowThumb.DesiredSize.Width,
            DragTabControl = parts.DragTabControl.DesiredSize.Width,
            AddTabButton = parts.AddTabButton.DesiredSize.Width,
            RightDragWindowThumb = parts.RightDragWindowThumb.DesiredSize.Width,
            RightContentControl = parts.RightContentControl.DesiredSize.Width,
        };

        double tabsHeight = Math.Max(parts.DragTabControl.DesiredSize.Height, finalSize.Height);

        double withoutTabsWidth = partsWidth.LeftContentControl
                                  + partsWidth.LeftDragWindowThumb
                                  + partsWidth.AddTabButton
                                  + partsWidth.RightDragWindowThumb
                                  + partsWidth.RightContentControl;
        
        double availableTabsWidth = finalSize.Width - withoutTabsWidth;

        //|                                      finalSize.Width                                        |
        //
        //   if (partsWidth.DragTabControl < availableTabsWidth):
        //|leftContent|leftThumb|tab1    |tab2    |addTabButton|         rightThumb        |rightContent|
        //
        //   else
        //|leftContent|leftThumb|tab1|tab2|tab3|tab4|tab5|tab6|tab7|addTabButton|rightThumb|rightContent|

        if (partsWidth.DragTabControl < availableTabsWidth)
        {
            ArrangeWhenTabsFit(parts, partsWidth, tabsHeight, finalSize.Width);
            return finalSize;
        }

        ArrangeWhenTabsUnfit(parts, partsWidth, tabsHeight, availableTabsWidth);
        return finalSize;
    }

    /// <summary>
    /// |leftContent|leftThumb|tab1    |tab2    |addTabButton|         rightThumb        |rightContent|
    /// </summary>
    private void ArrangeWhenTabsFit(Parts parts, PartsWidth widths, double tabsHeight, double finalWidth)
    {
        double x = 0;
        parts.LeftContentControl.Arrange(new Rect(x, 0, widths.LeftContentControl, tabsHeight));
        x += widths.LeftContentControl;
        parts.LeftDragWindowThumb.Arrange(new Rect(x, 0, widths.LeftDragWindowThumb, tabsHeight));
        x += widths.LeftDragWindowThumb;
        parts.DragTabControl.Arrange(new Rect(x, 0, widths.DragTabControl, tabsHeight));
        x += widths.DragTabControl;

        ArrangeCenterVertical(parts.AddTabButton, x, tabsHeight);
        x += widths.AddTabButton;

        double availableSpaceWidth = finalWidth
                                     - widths.LeftContentControl
                                     - widths.LeftDragWindowThumb
                                     - widths.DragTabControl
                                     - widths.AddTabButton
                                     - widths.RightContentControl;

        parts.RightDragWindowThumb.Arrange(new Rect(x, 0, availableSpaceWidth, tabsHeight));
        x += availableSpaceWidth;
        parts.RightContentControl.Arrange(new Rect(x, 0, widths.RightContentControl, tabsHeight));
    }

    /// <summary>
    /// |leftContent|leftThumb|tab1|tab2|tab3|tab4|tab5|tab6|tab7|addTabButton|rightThumb|rightContent|
    /// </summary>
    private void ArrangeWhenTabsUnfit(Parts parts, PartsWidth widths, double tabsHeight,
        double availableTabsWidth)
    {
        double x = 0;
        parts.LeftContentControl.Arrange(new Rect(x, 0, widths.LeftContentControl, tabsHeight));
        x += widths.LeftContentControl;
        parts.LeftDragWindowThumb.Arrange(new Rect(x, 0, widths.LeftDragWindowThumb, tabsHeight));
        x += widths.LeftDragWindowThumb;

        parts.DragTabControl.Arrange(new Rect(x, 0, availableTabsWidth, tabsHeight));
        x += availableTabsWidth;

        ArrangeCenterVertical(parts.AddTabButton, x, tabsHeight);
        x += widths.AddTabButton;

        parts.RightDragWindowThumb.Arrange(new Rect(x, 0, widths.RightDragWindowThumb, tabsHeight));
        x += widths.RightDragWindowThumb;
        parts.RightContentControl.Arrange(new Rect(x, 0, widths.RightContentControl, tabsHeight));
    }

    private static void ArrangeCenterVertical(Layoutable control, double x, double fullHeight)
    {
        double width = control.DesiredSize.Width;
        double height = control.DesiredSize.Height;

        double y = (fullHeight - height) / 2;

        control.Arrange(new Rect(x, y, width, height));
    }

    private readonly struct Parts
    {
        public Control LeftContentControl { get; init; }
        public Control LeftDragWindowThumb { get; init; }
        public Control DragTabControl { get; init; }
        public Control AddTabButton { get; init; }
        public Control RightDragWindowThumb { get; init; }
        public Control RightContentControl { get; init; }
    }

    private readonly struct PartsWidth
    {
        public double LeftContentControl { get; init; }
        public double LeftDragWindowThumb { get; init; }
        public double DragTabControl { get; init; }
        public double AddTabButton { get; init; }
        public double RightDragWindowThumb { get; init; }
        public double RightContentControl { get; init; }
    }
}