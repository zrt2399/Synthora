using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace Synthora.Controls
{
    public enum ItemsAlignment
    {
        /// <summary>
        /// Items are laid out so the first one in each column/row touches the top/left of the panel.
        /// </summary>
        Start,
        /// <summary>
        /// Items are laid out so that each column/row is centred vertically/horizontally within the panel.
        /// </summary>
        Center,
        /// <summary>
        /// Items are laid out so the last one in each column/row touches the bottom/right of the panel.
        /// </summary>
        End
    }

    public class SpacingWrapPanel : Panel, INavigableContainer
    {
        private struct UVSize
        {
            private Orientation _orientation;
            internal double U;
            internal double V;

            internal double Width
            {
                get => _orientation == Orientation.Horizontal ? U : V;
                set
                {
                    if (_orientation == Orientation.Horizontal)
                    {
                        U = value;
                    }
                    else
                    {
                        V = value;
                    }
                }
            }

            internal double Height
            {
                get => _orientation == Orientation.Horizontal ? V : U;
                set
                {
                    if (_orientation == Orientation.Horizontal)
                    {
                        V = value;
                    }
                    else
                    {
                        U = value;
                    }
                }
            }

            internal UVSize(Orientation orientation, double width, double height)
            {
                U = V = 0d;
                _orientation = orientation;
                Width = width;
                Height = height;
            }

            internal UVSize(Orientation orientation)
            {
                U = V = 0d;
                _orientation = orientation;
            }
        }

        public static readonly StyledProperty<double> HorizontalSpacingProperty =
            AvaloniaProperty.Register<SpacingWrapPanel, double>(nameof(HorizontalSpacing));

        public static readonly StyledProperty<double> VerticalSpacingProperty =
            AvaloniaProperty.Register<SpacingWrapPanel, double>(nameof(VerticalSpacing));

        public static readonly StyledProperty<Orientation> OrientationProperty =
            WrapPanel.OrientationProperty.AddOwner<SpacingWrapPanel>();

        public static readonly StyledProperty<ItemsAlignment> ItemsAlignmentProperty =
            AvaloniaProperty.Register<SpacingWrapPanel, ItemsAlignment>(nameof(ItemsAlignment), defaultValue: ItemsAlignment.Start);

        public static readonly StyledProperty<double> ItemWidthProperty =
            WrapPanel.ItemWidthProperty.AddOwner<SpacingWrapPanel>();

        public static readonly StyledProperty<double> ItemHeightProperty =
            WrapPanel.ItemHeightProperty.AddOwner<SpacingWrapPanel>();

        static SpacingWrapPanel()
        {
            AffectsMeasure<WrapPanel>(HorizontalSpacingProperty, VerticalSpacingProperty, OrientationProperty, ItemWidthProperty, ItemHeightProperty);
            AffectsArrange<WrapPanel>(ItemsAlignmentProperty);
        }

        public double HorizontalSpacing
        {
            get => GetValue(HorizontalSpacingProperty);
            set => SetValue(HorizontalSpacingProperty, value);
        }

        public double VerticalSpacing
        {
            get => GetValue(VerticalSpacingProperty);
            set => SetValue(VerticalSpacingProperty, value);
        }

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public ItemsAlignment ItemsAlignment
        {
            get => GetValue(ItemsAlignmentProperty);
            set => SetValue(ItemsAlignmentProperty, value);
        }

        public double ItemWidth
        {
            get => GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public double ItemHeight
        {
            get => GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        IInputElement? INavigableContainer.GetControl(NavigationDirection direction, IInputElement? from, bool wrap)
        {
            Orientation orientation = Orientation;
            var children = Children;
            bool horiz = orientation == Orientation.Horizontal;
            int index = ((from != null) ? Children.IndexOf((Control)from) : (-1));
            switch (direction)
            {
                case NavigationDirection.First:
                    index = 0;
                    break;
                case NavigationDirection.Last:
                    index = children.Count - 1;
                    break;
                case NavigationDirection.Next:
                    index++;
                    break;
                case NavigationDirection.Previous:
                    index--;
                    break;
                case NavigationDirection.Left:
                    index = horiz ? (index - 1) : (-1);
                    break;
                case NavigationDirection.Right:
                    index = horiz ? (index + 1) : (-1);
                    break;
                case NavigationDirection.Up:
                    index = horiz ? (-1) : (index - 1);
                    break;
                case NavigationDirection.Down:
                    index = horiz ? (-1) : (index + 1);
                    break;
            }

            if (index >= 0 && index < children.Count)
            {
                return children[index];
            }

            return null;
        }

        //fix: https://github.com/AvaloniaUI/Avalonia/issues/18814
        private bool GreaterThan(double value1, double value2, double baseEpsilon = 1e-10)
        {
            var toplevel = TopLevel.GetTopLevel(this);
            double dpiScale = toplevel?.RenderScaling ?? 1d;
            int dpiTimes100 = (int)(dpiScale * 100);
            bool isNonEvenScale = dpiTimes100 % 2 != 0;
            double epsilon = isNonEvenScale ? baseEpsilon * 8 : baseEpsilon;

            double diff = value1 - value2;
            double max = Math.Max(Math.Abs(value1), Math.Abs(value2));
            return diff > Math.Max(epsilon, max * epsilon);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            double horizontalSpacing = HorizontalSpacing;
            double verticalSpacing = VerticalSpacing;
            Orientation orientation = Orientation;
            var children = Children;
            UVSize curLineSize = new UVSize(orientation);
            UVSize panelSize = new UVSize(orientation);
            UVSize uvConstraint = new UVSize(orientation, constraint.Width, constraint.Height);
            bool itemWidthSet = !double.IsNaN(itemWidth);
            bool itemHeightSet = !double.IsNaN(itemHeight);
            bool itemExists = false;
            bool lineExists = false;
            Size availableSize = new Size(itemWidthSet ? itemWidth : constraint.Width, itemHeightSet ? itemHeight : constraint.Height);

            for (int i = 0; i < children.Count; i++)
            {
                Control child = children[i];
                child.Measure(availableSize);
                UVSize childSize = new UVSize(orientation, itemWidthSet ? itemWidth : child.DesiredSize.Width, itemHeightSet ? itemHeight : child.DesiredSize.Height);
                double nextSpacing = (itemExists && child.IsVisible) ? horizontalSpacing : 0;
                if (GreaterThan(curLineSize.U + childSize.U + nextSpacing, uvConstraint.U))
                {
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V + (lineExists ? verticalSpacing : 0);
                    curLineSize = childSize;
                    itemExists = child.IsVisible;
                    lineExists = true;
                }
                else
                {
                    curLineSize.U += childSize.U + nextSpacing;
                    curLineSize.V = Math.Max(childSize.V, curLineSize.V);
                    itemExists |= child.IsVisible;
                }
            }

            panelSize.U = Math.Max(curLineSize.U, panelSize.U);
            panelSize.V += curLineSize.V + (lineExists ? verticalSpacing : 0);
            return new Size(panelSize.Width, panelSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            double horizontalSpacing = HorizontalSpacing;
            double verticalSpacing = VerticalSpacing;
            Orientation orientation = Orientation;
            bool isHorizontal = orientation == Orientation.Horizontal;
            var children = Children;
            int firstInLine = 0;
            double accumulatedV = 0;
            double itemU = (isHorizontal ? itemWidth : itemHeight);
            UVSize curLineSize = new UVSize(orientation);
            UVSize uvFinalSize = new UVSize(orientation, finalSize.Width, finalSize.Height);
            bool itemWidthSet = !double.IsNaN(itemWidth);
            bool itemHeightSet = !double.IsNaN(itemHeight);
            bool itemExists = false;
            bool lineExists = false;

            for (int i = 0; i < children.Count; i++)
            {
                Control child = children[i];
                UVSize childSize = new UVSize(orientation, itemWidthSet ? itemWidth : child.DesiredSize.Width, itemHeightSet ? itemHeight : child.DesiredSize.Height);
                double nextSpacing = (itemExists && child.IsVisible) ? horizontalSpacing : 0;
                if (GreaterThan(curLineSize.U + childSize.U + nextSpacing, uvFinalSize.U))
                {
                    accumulatedV += (lineExists ? verticalSpacing : 0);
                    ArrangeLine(curLineSize.V, firstInLine, i);
                    accumulatedV += curLineSize.V;
                    curLineSize = childSize;
                    firstInLine = i;
                    itemExists = child.IsVisible;
                    lineExists = true;
                }
                else
                {
                    curLineSize.U += childSize.U + nextSpacing;
                    curLineSize.V = Math.Max(childSize.V, curLineSize.V);
                    itemExists |= child.IsVisible;
                }
            }

            if (firstInLine < children.Count)
            {
                accumulatedV += (lineExists ? verticalSpacing : 0);
                ArrangeLine(curLineSize.V, firstInLine, children.Count);
            }

            return finalSize;

            void ArrangeLine(double lineV, int start, int end)
            {
                bool useItemU = isHorizontal ? itemWidthSet : itemHeightSet;
                double u = 0;
                if (ItemsAlignment != ItemsAlignment.Start)
                {
                    double totalU = 0 - horizontalSpacing;
                    for (int i = start; i < end; i++)
                    {
                        totalU += GetChildU(i) + ((!children[i].IsVisible) ? 0 : horizontalSpacing);
                    }

                    u = ItemsAlignment switch
                    {
                        ItemsAlignment.Center => (uvFinalSize.U - totalU) / 2.0,
                        ItemsAlignment.End => uvFinalSize.U - totalU,
                        ItemsAlignment.Start => 0,
                        _ => throw new ArgumentOutOfRangeException(nameof(ItemsAlignment), ItemsAlignment, null),
                    };
                }

                for (int i = start; i < end; i++)
                {
                    double layoutSlotU = GetChildU(i);
                    children[i].Arrange(isHorizontal ? new Rect(u, accumulatedV, layoutSlotU, lineV) : new Rect(accumulatedV, u, lineV, layoutSlotU));
                    u += layoutSlotU + ((!children[i].IsVisible) ? 0 : horizontalSpacing);
                }

                double GetChildU(int i)
                {
                    if (useItemU)
                    {
                        return itemU;
                    }

                    return isHorizontal ? children[i].DesiredSize.Width : children[i].DesiredSize.Height;
                }
            }
        }
    }
}