using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Synthora
{
    public class SynthoraTheme : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynthoraTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SynthoraTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
} 