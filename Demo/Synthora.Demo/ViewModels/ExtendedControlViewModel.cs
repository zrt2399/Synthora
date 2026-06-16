using System.Collections;
using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Synthora.Demo.Models;

namespace Synthora.Demo.ViewModels
{
    public partial class ExtendedControlViewModel : TreeMenuDemoItem
    {
        [ObservableProperty]
        public partial IList? SelectedFiles { get; set; }

        public IReadOnlyList<FilePickerFileType> FilePickerFileTypes { get; } = new List<FilePickerFileType>
        {
            new FilePickerFileType("Image Files")
            {
                Patterns = ["*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp"]
            },
            new FilePickerFileType("Text Files")
            {
                Patterns = ["*.txt"]
            },
            new FilePickerFileType("All Files")
            {
                Patterns = ["*"]
            }
        };
        
        public ExtendedControlViewModel()
        {
            IconKind = MaterialIconKind.Widgets;
            Description = "Badge, ShadowChrome, GroupBoxEx, PathPicker, SpacingWrapPanel, EmptyBox";
        }
    }
}