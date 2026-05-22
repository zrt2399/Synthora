using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class CalendarViewModel: TreeMenuDemoItem
    {
        public CalendarViewModel()
        {
            IconKind = MaterialIconKind.CalendarMonth;
            Description = "CalendarDatePicker, DatePicker, TimePicker, Calendar";
        }
    }
}