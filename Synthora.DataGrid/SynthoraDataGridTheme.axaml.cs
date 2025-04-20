using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Synthora.DataGrid
{
    public class SynthoraDataGridTheme : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynthoraDataGridTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SynthoraDataGridTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}