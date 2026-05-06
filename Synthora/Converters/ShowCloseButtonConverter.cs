using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Synthora.Converters;

internal class ShowCloseButtonConverter : IMultiValueConverter
{
    public static ShowCloseButtonConverter Instance { get; } = new ShowCloseButtonConverter();

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        // [0] is owning DragTabControl ShowCloseButton value.
        // [1] is owning DragTabControl FixedHeaderCount value.
        // [2] is item LogicalIndex
        if (values.Count == 3)
        {
            if (values[0] is not bool showDefaultCloseButton ||
                values[1] is not int fixedHeaderCount ||
                values[2] is not int logicalIndex)
            {
                return false;
            }

            return showDefaultCloseButton && logicalIndex >= fixedHeaderCount;
        }

        return false;
    }
}