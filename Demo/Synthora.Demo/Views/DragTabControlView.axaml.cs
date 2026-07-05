using Avalonia.Controls;
using Synthora.Controls;

namespace Synthora.Demo.Views
{
    public partial class DragTabControlView : UserControl
    {
        public DragTabControlView()
        {
            InitializeComponent();
            DragTabControl.NewItemFactory = AddDragTabItem;
        }

        private DragTabItem AddDragTabItem()
        {
            return new DragTabItem
            {
                Header = $"Tab {DragTabControl.Items.Count + 1}",
                Content = $"Content of Tab {DragTabControl.Items.Count + 1}"
            };
        }
    }
}