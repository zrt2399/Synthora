using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Synthora.Utils;

namespace Synthora.Controls
{
    public class PathPicker : TemplatedControl
    {
        private TextBox? PART_TextBox;

        public static readonly StyledProperty<string?> TitleProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(Title));

        public static readonly StyledProperty<string?> SuggestedStartLocationProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SuggestedStartLocation));

        public static readonly StyledProperty<string?> SuggestedFileNameProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SuggestedFileName));

        public static readonly StyledProperty<string?> DefaultExtensionProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(DefaultExtension));

        public static readonly StyledProperty<bool> AllowMultipleProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(AllowMultiple));

        public static readonly StyledProperty<string?> SelectedPathProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SelectedPath), defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<IList?> SelectedPathsProperty =
            AvaloniaProperty.Register<PathPicker, IList?>(nameof(SelectedPaths), defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<bool> UseFolderDialogProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(UseFolderDialog));

        public static readonly StyledProperty<bool> UseSaveDialogProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(UseSaveDialog));

        public static readonly StyledProperty<IReadOnlyList<FilePickerFileType>> FilePickerFileTypesProperty =
            AvaloniaProperty.Register<PathPicker, IReadOnlyList<FilePickerFileType>>(nameof(FilePickerFileTypes));

        public static readonly StyledProperty<string?> WatermarkProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(Watermark));

        public static readonly StyledProperty<object?> BrowseButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(BrowseButtonContent), " ... ");

        public static readonly StyledProperty<object?> ExploreButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(ExploreButtonContent));

        public static readonly StyledProperty<object?> OpenButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(OpenButtonContent));

        public static readonly StyledProperty<object?> BrowseButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(BrowseButtonToolTip));

        public static readonly StyledProperty<object?> ExploreButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(ExploreButtonToolTip));

        public static readonly StyledProperty<object?> OpenButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(OpenButtonToolTip));

        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            ContentControl.HorizontalContentAlignmentProperty.AddOwner<PathPicker>();

        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            ContentControl.VerticalContentAlignmentProperty.AddOwner<PathPicker>();

        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            AvaloniaProperty.Register<PathPicker, TextWrapping>(nameof(TextWrapping));

        public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
            AvaloniaProperty.Register<PathPicker, TextAlignment>(nameof(TextAlignment));

        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(IsReadOnly), true);

        public static readonly StyledProperty<Thickness> SpacingProperty =
            AvaloniaProperty.Register<PathPicker, Thickness>(nameof(Spacing), new Thickness(4, 0, 0, 0));

        static PathPicker()
        {
            SelectedPathsProperty.Changed.AddClassHandler<PathPicker, IList?>((s, e) => OnSelectedPathsChanged(e));
        }

        /// <summary>
        /// Gets the path separator character used to separate paths.
        /// It is '|' on Windows and ':' on other platforms.
        /// </summary>
        public static string PathSeparator { get; } = OperatingSystem.IsWindows() ? "|" : ":";

        /// <summary>
        /// Gets or sets the title of the file dialog.
        /// </summary>
        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string? SuggestedStartLocation
        {
            get => GetValue(SuggestedStartLocationProperty);
            set => SetValue(SuggestedStartLocationProperty, value);
        }

        public string? SuggestedFileName
        {
            get => GetValue(SuggestedFileNameProperty);
            set => SetValue(SuggestedFileNameProperty, value);
        }

        public string? DefaultExtension
        {
            get => GetValue(DefaultExtensionProperty);
            set => SetValue(DefaultExtensionProperty, value);
        }

        public bool AllowMultiple
        {
            get => GetValue(AllowMultipleProperty);
            set => SetValue(AllowMultipleProperty, value);
        }

        public string? SelectedPath
        {
            get => GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public IList? SelectedPaths
        {
            get => GetValue(SelectedPathsProperty);
            set => SetValue(SelectedPathsProperty, value);
        }

        public bool UseFolderDialog
        {
            get => GetValue(UseFolderDialogProperty);
            set => SetValue(UseFolderDialogProperty, value);
        }

        public bool UseSaveDialog
        {
            get => GetValue(UseSaveDialogProperty);
            set => SetValue(UseSaveDialogProperty, value);
        }

        public IReadOnlyList<FilePickerFileType> FilePickerFileTypes
        {
            get => GetValue(FilePickerFileTypesProperty);
            set => SetValue(FilePickerFileTypesProperty, value);
        }

        public string? Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public object? BrowseButtonContent
        {
            get => GetValue(BrowseButtonContentProperty);
            set => SetValue(BrowseButtonContentProperty, value);
        }

        public object? ExploreButtonContent
        {
            get => GetValue(ExploreButtonContentProperty);
            set => SetValue(ExploreButtonContentProperty, value);
        }

        public object? OpenButtonContent
        {
            get => GetValue(OpenButtonContentProperty);
            set => SetValue(OpenButtonContentProperty, value);
        }

        public object? BrowseButtonToolTip
        {
            get => GetValue(BrowseButtonToolTipProperty);
            set => SetValue(BrowseButtonToolTipProperty, value);
        }

        public object? ExploreButtonToolTip
        {
            get => GetValue(ExploreButtonToolTipProperty);
            set => SetValue(ExploreButtonToolTipProperty, value);
        }

        public object? OpenButtonToolTip
        {
            get => GetValue(OpenButtonToolTipProperty);
            set => SetValue(OpenButtonToolTipProperty, value);
        }

        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        public TextWrapping TextWrapping
        {
            get => GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public TextAlignment TextAlignment
        {
            get => GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public bool IsReadOnly
        {
            get => GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public Thickness Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        private static void OnSelectedPathsChanged(AvaloniaPropertyChangedEventArgs<IList?> e)
        {
            var pathPicker = (PathPicker)e.Sender;
            var oldSelectedPaths = e.OldValue.Value;
            var newSelectedPaths = e.NewValue.Value;
            pathPicker.OnSelectedPathsChanged(oldSelectedPaths, newSelectedPaths);
        }

        protected virtual void OnSelectedPathsChanged(IList? oldSelectedPaths, IList? newSelectedPaths)
        {
            SetCurrentValue(SelectedPathProperty, newSelectedPaths == null || newSelectedPaths.Count == 0 ? string.Empty : string.Join(PathSeparator, newSelectedPaths.OfType<string>()));
        }

        private async Task SetCommonOptionsAsync(PickerOptions pickerOptions, TopLevel topLevel)
        {
            if (pickerOptions.Title != null)
            {
                pickerOptions.Title = Title;
            }
            if (SuggestedStartLocation != null)
            {
                pickerOptions.SuggestedStartLocation = await topLevel.StorageProvider.TryGetFolderFromPathAsync(SuggestedStartLocation);
            }
            if (SuggestedFileName != null)
            {
                pickerOptions.SuggestedFileName = SuggestedFileName;
            }
        }

        public async void Browse()
        {
            if (TopLevel.GetTopLevel(this) is not TopLevel topLevel)
            {
                return;
            }
            if (UseFolderDialog)
            {
                var options = new FolderPickerOpenOptions
                {
                    AllowMultiple = AllowMultiple
                };
                await SetCommonOptionsAsync(options, topLevel);
                var storageFolders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);

                if (storageFolders.Count > 0)
                {
                    List<string> folders = new List<string>(storageFolders.Count);
                    foreach (var item in storageFolders)
                    {
                        folders.Add(item.TryGetLocalPath() ?? string.Empty);
                    }
                    SetCurrentValue(SelectedPathsProperty, folders);
                }
            }
            else
            {
                if (UseSaveDialog)
                {
                    var options = new FilePickerSaveOptions
                    {
                        DefaultExtension = DefaultExtension,
                        FileTypeChoices = FilePickerFileTypes
                    };
                    await SetCommonOptionsAsync(options, topLevel);
                    using var storageFile = await topLevel.StorageProvider.SaveFilePickerAsync(options);
                    if (storageFile != null)
                    {
                        SetCurrentValue(SelectedPathsProperty, new List<string>()
                        {
                            storageFile.TryGetLocalPath() ?? string.Empty
                        });
                    }
                }
                else
                {
                    var options = new FilePickerOpenOptions
                    {
                        AllowMultiple = AllowMultiple,
                        FileTypeFilter = FilePickerFileTypes
                    };
                    await SetCommonOptionsAsync(options, topLevel);
                    var storageFiles = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                    if (storageFiles.Count > 0)
                    {
                        List<string> files = new List<string>(storageFiles.Count);
                        foreach (var item in storageFiles)
                        {
                            files.Add(item.TryGetLocalPath() ?? string.Empty);
                        }
                        SetCurrentValue(SelectedPathsProperty, files);
                    }
                }
            }
        }

        public void Explore()
        {
            if (SelectedPaths?.Count > 0 && SelectedPaths[0] is string path)
            {
                PathUtils.OpenFileLocation(path);
            }
        }

        public void Open()
        {
            if (SelectedPaths?.Count > 0 && SelectedPaths[0] is string path)
            {
                PathUtils.OpenFlie(path);
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PART_TextBox = e.NameScope.Find<TextBox>(nameof(PART_TextBox));
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            if (!e.Handled && PART_TextBox != null)
            {
                if (Equals(e.Source, this))
                {
                    PART_TextBox.Focus();
                    PART_TextBox.SelectAll();
                }
            }
            base.OnGotFocus(e);
        }
    }
}