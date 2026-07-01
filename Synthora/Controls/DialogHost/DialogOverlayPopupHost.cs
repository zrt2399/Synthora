using Avalonia;
using Avalonia.Controls;

namespace Synthora.Controls
{
    /// <summary>
    /// Popup host used by <see cref="DialogHost"/> to display dialog content on the overlay layer.
    /// </summary>
    public class DialogOverlayPopupHost : ContentControl
    {
        private bool _isOpen;

        /// <summary>
        /// Defines the <see cref="DialogHostInstance"/> property.
        /// </summary>
        public static readonly StyledProperty<DialogHostInstance?> DialogHostInstanceProperty =
            AvaloniaProperty.Register<DialogOverlayPopupHost, DialogHostInstance?>(nameof(DialogHostInstance));

        /// <summary>
        /// Defines the <see cref="DisableOpeningAnimation"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> DisableOpeningAnimationProperty =
            AvaloniaProperty.Register<DialogOverlayPopupHost, bool>(nameof(DisableOpeningAnimation));

        /// <summary>
        /// Gets or sets the dialog instance rendered by this popup host.
        /// </summary>
        public DialogHostInstance? DialogHostInstance
        {
            get => GetValue(DialogHostInstanceProperty);
            set => SetValue(DialogHostInstanceProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="IsOpen"/> property.
        /// </summary>
        public static readonly DirectProperty<DialogOverlayPopupHost, bool> IsOpenProperty =
            AvaloniaProperty.RegisterDirect<DialogOverlayPopupHost, bool>(nameof(IsOpen), o => o.IsOpen);

        /// <summary>
        /// Gets or sets whether opening animations are disabled.
        /// </summary>
        public bool DisableOpeningAnimation
        {
            get => GetValue(DisableOpeningAnimationProperty);
            set => SetValue(DisableOpeningAnimationProperty, value);
        }

        /// <summary>
        /// Gets whether this popup host is currently open.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            internal set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

        /// <summary>
        /// Gets the rendered dialog as an alert dialog instance when applicable.
        /// </summary>
        public AlertDialogInstance? AlertDialogInstance => DialogHostInstance as AlertDialogInstance;

        /// <summary>
        /// Closes the dialog rendered by this popup host.
        /// </summary>
        /// <param name="parameter">The optional result passed to the dialog close operation.</param>
        public void Close(object? parameter = null)
        {
            DialogHostInstance?.Close(parameter);
        }

        internal void ClearDialogReferences()
        {
            ClearValue(ContentProperty);
            ClearValue(ContentTemplateProperty);
            ClearValue(TemplateProperty);
            ClearValue(DialogHostInstanceProperty);
        }
    }
}