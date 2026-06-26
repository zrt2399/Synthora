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
    /// <summary>
    /// Represents a control for selecting, displaying, opening, and exploring file system paths.
    /// </summary>
    public class PathPicker : TemplatedControl
    {
        private TextBox? _textBox;

        /// <summary>
        /// Defines the <see cref="Title"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> TitleProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(Title));

        /// <summary>
        /// Defines the <see cref="SuggestedStartLocation"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> SuggestedStartLocationProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SuggestedStartLocation));

        /// <summary>
        /// Defines the <see cref="SuggestedFileName"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> SuggestedFileNameProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SuggestedFileName));

        /// <summary>
        /// Defines the <see cref="DefaultExtension"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> DefaultExtensionProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(DefaultExtension));

        /// <summary>
        /// Defines the <see cref="AllowMultiple"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> AllowMultipleProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(AllowMultiple));

        /// <summary>
        /// Defines the <see cref="SelectedPath"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> SelectedPathProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(SelectedPath), defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Defines the <see cref="SelectedPaths"/> property.
        /// </summary>
        public static readonly StyledProperty<IList?> SelectedPathsProperty =
            AvaloniaProperty.Register<PathPicker, IList?>(nameof(SelectedPaths), defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Defines the <see cref="UseFolderDialog"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> UseFolderDialogProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(UseFolderDialog));

        /// <summary>
        /// Defines the <see cref="UseSaveDialog"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> UseSaveDialogProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(UseSaveDialog));

        /// <summary>
        /// Defines the <see cref="FilePickerFileTypes"/> property.
        /// </summary>
        public static readonly StyledProperty<IReadOnlyList<FilePickerFileType>> FilePickerFileTypesProperty =
            AvaloniaProperty.Register<PathPicker, IReadOnlyList<FilePickerFileType>>(nameof(FilePickerFileTypes));

        /// <summary>
        /// Defines the <see cref="PlaceholderText"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> PlaceholderTextProperty =
            AvaloniaProperty.Register<PathPicker, string?>(nameof(PlaceholderText));

        /// <summary>
        /// Defines the <see cref="PlaceholderForeground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
            AvaloniaProperty.Register<TextBox, IBrush?>(nameof(PlaceholderForeground));

        /// <summary>
        /// Defines the <see cref="BrowseButtonContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> BrowseButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(BrowseButtonContent), " ... ");

        /// <summary>
        /// Defines the <see cref="ExploreButtonContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> ExploreButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(ExploreButtonContent));

        /// <summary>
        /// Defines the <see cref="OpenButtonContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> OpenButtonContentProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(OpenButtonContent));

        /// <summary>
        /// Defines the <see cref="BrowseButtonToolTip"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> BrowseButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(BrowseButtonToolTip));

        /// <summary>
        /// Defines the <see cref="ExploreButtonToolTip"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> ExploreButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(ExploreButtonToolTip));

        /// <summary>
        /// Defines the <see cref="OpenButtonToolTip"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> OpenButtonToolTipProperty =
            AvaloniaProperty.Register<PathPicker, object?>(nameof(OpenButtonToolTip));

        /// <summary>
        /// Defines the <see cref="HorizontalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            ContentControl.HorizontalContentAlignmentProperty.AddOwner<PathPicker>();

        /// <summary>
        /// Defines the <see cref="VerticalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            ContentControl.VerticalContentAlignmentProperty.AddOwner<PathPicker>();

        /// <summary>
        /// Defines the <see cref="TextWrapping"/> property.
        /// </summary>
        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            AvaloniaProperty.Register<PathPicker, TextWrapping>(nameof(TextWrapping));

        /// <summary>
        /// Defines the <see cref="TextAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
            AvaloniaProperty.Register<PathPicker, TextAlignment>(nameof(TextAlignment));

        /// <summary>
        /// Defines the <see cref="IsReadOnly"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<PathPicker, bool>(nameof(IsReadOnly), true);

        /// <summary>
        /// Defines the <see cref="Spacing"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> SpacingProperty =
            AvaloniaProperty.Register<PathPicker, Thickness>(nameof(Spacing), new Thickness(6, 0, 0, 0));

        static PathPicker()
        {
            SelectedPathsProperty.Changed.AddClassHandler<PathPicker, IList?>((s, e) => OnSelectedPathsChanged(e));
        }

        /// <summary>
        /// Gets the path separator used to serialize multiple selected paths into <see cref="SelectedPath"/>.
        /// Uses the Unit Separator control character (0x1F) so the separator is consistent across platforms and avoids NUL-terminator issues in other languages.
        /// </summary>
        public const string PathSeparator = "\u001F";

        /// <summary>
        /// Gets or sets the text that appears in the title bar of a picker.
        /// </summary>
        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the suggested start location for the picker dialog.
        /// </summary>
        public string? SuggestedStartLocation
        {
            get => GetValue(SuggestedStartLocationProperty);
            set => SetValue(SuggestedStartLocationProperty, value);
        }

        /// <summary>
        /// Gets or sets the file name that the file picker suggests to the user.
        /// </summary>
        public string? SuggestedFileName
        {
            get => GetValue(SuggestedFileNameProperty);
            set => SetValue(SuggestedFileNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the default extension used by save dialogs.
        /// </summary>
        public string? DefaultExtension
        {
            get => GetValue(DefaultExtensionProperty);
            set => SetValue(DefaultExtensionProperty, value);
        }

        /// <summary>
        /// Gets or sets whether multiple paths can be selected.
        /// </summary>
        public bool AllowMultiple
        {
            get => GetValue(AllowMultipleProperty);
            set => SetValue(AllowMultipleProperty, value);
        }

        /// <summary>
        /// Gets or sets the serialized selected path value.
        /// </summary>
        public string? SelectedPath
        {
            get => GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        /// <summary>
        /// Gets or sets the selected path collection.
        /// </summary>
        public IList? SelectedPaths
        {
            get => GetValue(SelectedPathsProperty);
            set => SetValue(SelectedPathsProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the picker opens a folder selection dialog.
        /// </summary>
        public bool UseFolderDialog
        {
            get => GetValue(UseFolderDialogProperty);
            set => SetValue(UseFolderDialogProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the picker opens a save file dialog.
        /// </summary>
        public bool UseSaveDialog
        {
            get => GetValue(UseSaveDialogProperty);
            set => SetValue(UseSaveDialogProperty, value);
        }

        /// <summary>
        /// Gets or sets the file type filters used by file picker dialogs.
        /// </summary>
        public IReadOnlyList<FilePickerFileType> FilePickerFileTypes
        {
            get => GetValue(FilePickerFileTypesProperty);
            set => SetValue(FilePickerFileTypesProperty, value);
        }

        /// <summary>
        /// Gets or sets the placeholder text displayed when no path is selected.
        /// </summary>
        public string? PlaceholderText
        {
            get => GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used for placeholder text.
        /// </summary>
        public IBrush? PlaceholderForeground
        {
            get => GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the content of the browse button.
        /// </summary>
        public object? BrowseButtonContent
        {
            get => GetValue(BrowseButtonContentProperty);
            set => SetValue(BrowseButtonContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content of the explore button.
        /// </summary>
        public object? ExploreButtonContent
        {
            get => GetValue(ExploreButtonContentProperty);
            set => SetValue(ExploreButtonContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content of the open button.
        /// </summary>
        public object? OpenButtonContent
        {
            get => GetValue(OpenButtonContentProperty);
            set => SetValue(OpenButtonContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the tooltip content of the browse button.
        /// </summary>
        public object? BrowseButtonToolTip
        {
            get => GetValue(BrowseButtonToolTipProperty);
            set => SetValue(BrowseButtonToolTipProperty, value);
        }

        /// <summary>
        /// Gets or sets the tooltip content of the explore button.
        /// </summary>
        public object? ExploreButtonToolTip
        {
            get => GetValue(ExploreButtonToolTipProperty);
            set => SetValue(ExploreButtonToolTipProperty, value);
        }

        /// <summary>
        /// Gets or sets the tooltip content of the open button.
        /// </summary>
        public object? OpenButtonToolTip
        {
            get => GetValue(OpenButtonToolTipProperty);
            set => SetValue(OpenButtonToolTipProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the displayed path content.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the displayed path content.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets how the displayed path text wraps.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get => GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        /// <summary>
        /// Gets or sets the alignment of the displayed path text.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the path text box is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get => GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between the displayed path and command buttons.
        /// </summary>
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

        /// <summary>
        /// Opens the configured picker dialog and updates the selected path values.
        /// </summary>
        public async void Browse()
        {
            if (TopLevel.GetTopLevel(this) is not { } topLevel)
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
                    var folders = new List<string>(storageFolders.Count);
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
                        var files = new List<string>(storageFiles.Count);
                        foreach (var item in storageFiles)
                        {
                            files.Add(item.TryGetLocalPath() ?? string.Empty);
                        }
                        SetCurrentValue(SelectedPathsProperty, files);
                    }
                }
            }
        }

        /// <summary>
        /// Opens the location of the first selected path in the platform file explorer.
        /// </summary>
        public void Explore()
        {
            if (SelectedPaths?.Count > 0 && SelectedPaths[0] is string path)
            {
                PathUtils.OpenFileLocation(path);
            }
        }

        /// <summary>
        /// Opens the first selected path with the platform default application.
        /// </summary>
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
            _textBox = e.NameScope.Find<TextBox>("PART_TextBox");
        }

        protected override void OnGotFocus(FocusChangedEventArgs e)
        {
            if (!e.Handled && _textBox != null)
            {
                if (Equals(e.Source, this))
                {
                    _textBox.Focus(e.NavigationMethod, e.KeyModifiers);
                    _textBox.SelectAll();
                }
            }
            base.OnGotFocus(e);
        }
    }
}