using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public class TextBlockViewModel : TreeMenuDemoItem
    {
        public TextBlockViewModel()
        {
            IconKind = MaterialIconKind.TextBoxOutline;
            Description = "TextBlock, SelectableTextBlock";
        }

        public string WrappingSample { get; } = "This paragraph demonstrates wrapping, spacing, and the default foreground resources in Synthora. It is useful for descriptions, helper text, and dense UI copy that still needs to remain readable across density modes.";

        public string CodeSample { get; } = "<SelectableTextBlock TextWrapping=\"Wrap\"\n                     Text=\"Copyable text sample\" />";
    }
}