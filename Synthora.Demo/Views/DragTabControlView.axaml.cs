using Avalonia.Controls;
using Synthora.Controls;

namespace Synthora.Demo.Views
{
    public partial class DragTabControlView : UserControl
    {
        public DragTabControlView()
        {
            InitializeComponent();
            dragTabControl.NewItemFactory = AddDragTabItem;
        }

        private DragTabItem AddDragTabItem()
        {
            return new DragTabItem
            {
                Header = $"Tab {dragTabControl.Items.Count + 1}",
                Content = $"Content of Tab {dragTabControl.Items.Count + 1}"
            };
        }
    }
}