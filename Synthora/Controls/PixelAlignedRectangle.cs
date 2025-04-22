using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Synthora.Controls
{
    public class PixelAlignedRectangle : Rectangle
    {
        protected override Geometry CreateDefiningGeometry()
        {
            var rect = new Rect(0, 0, double.IsNaN(Width) ? Bounds.Width : Width, double.IsNaN(Height) ? Bounds.Height : Height);
            return new RectangleGeometry(rect, RadiusX, RadiusY);
        }

        //public override void Render(DrawingContext context)
        //{
        //    var brush = Background;
        //    if (brush != null)
        //    {
        //        var rect = new Rect(0, 0, double.IsNaN(Width) ? Bounds.Width : Width, double.IsNaN(Height) ? Bounds.Height : Height);
        //        context.FillRectangle(brush, rect);
        //    }
        //}
    }
}