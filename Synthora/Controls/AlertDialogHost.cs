using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Layout;
using Avalonia.Media;
using Synthora.Overlays;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a single alert dialog instance currently managed by an <see cref="AlertDialogHost"/>.
    /// </summary>
    public class AlertDialogInstance
    {
        /// <summary>
        /// Gets the title displayed by the dialog.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Gets the message displayed by the dialog.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Gets the set of buttons displayed by the dialog.
        /// </summary>
        public DialogButton DialogButton { get; }

        /// <summary>
        /// Gets the icon type displayed by the dialog.
        /// </summary>
        public IconType IconType { get; }

        /// <summary>
        /// Gets a value indicating whether the dialog displays a close button.
        /// </summary>
        public bool ShowCloseButton { get; }

        /// <summary>
        /// Gets a value indicating whether the dialog footer content stretches to the available width.
        /// </summary>
        public bool IsFooterStretched { get; }

        /// <summary>
        /// Gets the maximum width of the dialog.
        /// </summary>
        public double MaxWidth { get; }

        /// <summary>
        /// Gets the maximum height of the dialog.
        /// </summary>
        public double MaxHeight { get; }

        /// <summary>
        /// Gets the horizontal alignment of the dialog.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; }

        /// <summary>
        /// Gets the vertical alignment of the dialog.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; }

        /// <summary>
        /// Gets the host that owns the dialog instance.
        /// </summary>
        public AlertDialogHost Host { get; }

        /// <summary>
        /// Gets the completion source used to publish the dialog result.
        /// </summary>
        public TaskCompletionSource<DialogResult> Tcs { get; } = new TaskCompletionSource<DialogResult>();

        /// <summary>
        /// Initializes a dialog instance from the supplied host and dialog arguments.
        /// </summary>
        public AlertDialogInstance(AlertDialogHost host, AlertDialogOptions options)
        {
            Host = host;
            Title = options.Title;
            Message = options.Message;
            DialogButton = options.DialogButton;
            IconType = options.IconType;
            ShowCloseButton = options.ShowCloseButton || DialogButton == DialogButton.None;
            IsFooterStretched = options.IsFooterStretched;
            MaxWidth = options.MaxWidth;
            MaxHeight = options.MaxHeight;
            HorizontalAlignment = options.HorizontalAlignment;
            VerticalAlignment = options.VerticalAlignment;
        }

        /// <summary>
        /// Closes the dialog with an <see cref="DialogResult.OK"/> result.
        /// </summary>
        public void OK() => Host.Close(this, DialogResult.OK);

        /// <summary>
        /// Closes the dialog with a <see cref="DialogResult.Cancel"/> result.
        /// </summary>
        public void Cancel() => Host.Close(this, DialogResult.Cancel);

        /// <summary>
        /// Closes the dialog with a <see cref="DialogResult.Yes"/> result.
        /// </summary>
        public void Yes() => Host.Close(this, DialogResult.Yes);

        /// <summary>
        /// Closes the dialog with a <see cref="DialogResult.No"/> result.
        /// </summary>
        public void No() => Host.Close(this, DialogResult.No);

        /// <summary>
        /// Closes the dialog with an <see cref="DialogResult.Abort"/> result.
        /// </summary>
        public void Abort() => Host.Close(this, DialogResult.Abort);

        /// <summary>
        /// Closes the dialog with a <see cref="DialogResult.None"/> result.
        /// </summary>
        public void None() => Host.Close(this, DialogResult.None);
    }

    /// <summary>
    /// Hosts alert dialogs and manages their overlay state.
    /// </summary>
    [PseudoClasses(pcOpen)]
    public class AlertDialogHost : ContentControl
    {
        private bool _isOpen;
        private const string pcOpen = ":open";
        private static readonly HashSet<AlertDialogHost> _loadedInstances = [];
        private readonly ObservableCollection<AlertDialogInstance> _dialogs = [];

        /// <summary>
        /// Defines the <see cref="BlurRadius"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BlurRadiusProperty =
            AvaloniaProperty.Register<AlertDialogHost, double>(nameof(BlurRadius));

        /// <summary>
        /// Defines the <see cref="Identifier"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> IdentifierProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Identifier));

        /// <summary>
        /// Defines the <see cref="OverlayBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> OverlayBackgroundProperty =
            AvaloniaProperty.Register<AlertDialogHost, IBrush?>(nameof(OverlayBackground));

        /// <summary>
        /// Defines the <see cref="DialogBoxShadow"/> property.
        /// </summary>
        public static readonly StyledProperty<BoxShadows> DialogBoxShadowProperty =
            AvaloniaProperty.Register<AlertDialogHost, BoxShadows>(nameof(DialogBoxShadow));

        /// <summary>
        /// Defines the <see cref="DisableOpeningAnimation"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> DisableOpeningAnimationProperty =
            AvaloniaProperty.Register<AlertDialogHost, bool>(nameof(DisableOpeningAnimation));

        /// <summary>
        /// Defines the <see cref="IsOpen"/> property.
        /// </summary>
        public static readonly DirectProperty<AlertDialogHost, bool> IsOpenProperty =
            AvaloniaProperty.RegisterDirect<AlertDialogHost, bool>(nameof(IsOpen), o => o.IsOpen);

        /// <summary>
        /// Gets the dialogs currently displayed by the host.
        /// </summary>
        public IReadOnlyList<AlertDialogInstance> Dialogs => _dialogs;

        /// <summary>
        /// Gets or sets the blur radius applied behind open dialogs.
        /// </summary>
        public double BlurRadius
        {
            get => GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the identifier used to select this host when opening dialogs.
        /// </summary>
        public string? Identifier
        {
            get => GetValue(IdentifierProperty);
            set => SetValue(IdentifierProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used to paint the dialog overlay.
        /// </summary>
        public IBrush? OverlayBackground
        {
            get => GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the box shadow applied to dialogs.
        /// </summary>
        public BoxShadows DialogBoxShadow
        {
            get => GetValue(DialogBoxShadowProperty);
            set => SetValue(DialogBoxShadowProperty, value);
        }

        /// <summary>
        /// Gets or sets whether opening animations are disabled.
        /// </summary>
        public bool DisableOpeningAnimation
        {
            get => GetValue(DisableOpeningAnimationProperty);
            set => SetValue(DisableOpeningAnimationProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the host currently has an open dialog.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            internal set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertDialogHost"/> class.
        /// </summary>
        public AlertDialogHost()
        {
            _dialogs.CollectionChanged += (s, e) =>
            {
                IsOpen = _dialogs.Count > 0;
                PseudoClasses.Set(pcOpen, IsOpen);
            };
        }

        internal static AlertDialogHost GetInstance(string? dialogIdentifier)
        {
            if (_loadedInstances.Count == 0)
            {
                throw new InvalidOperationException("No loaded AlertDialogHost instances.");
            }

            var targets = _loadedInstances.Where(x => dialogIdentifier == null || Equals(x.Identifier, dialogIdentifier)).ToList();
            if (targets.Count == 0)
            {
                throw new InvalidOperationException($"No loaded AlertDialogHost have an {nameof(Identifier)} property matching {nameof(dialogIdentifier)} ('{dialogIdentifier}') argument.");
            }

            if (targets.Count > 1)
            {
                throw new InvalidOperationException("Multiple viable AlertDialogHosts. Specify a unique Identifier on each AlertDialogHost, especially where multiple Windows are a concern.");
            }

            return targets[0];
        }

        internal async Task<DialogResult> Show(AlertDialogOptions alertDialogOptions)
        {
            var dialog = new AlertDialogInstance(this, alertDialogOptions);
            _dialogs.Add(dialog);

            return await dialog.Tcs.Task;
        }

        internal void Close(AlertDialogInstance dialogInstance, DialogResult dialogResult)
        {
            if (_dialogs.Contains(dialogInstance))
            {
                _dialogs.Remove(dialogInstance);
                dialogInstance.Tcs.TrySetResult(dialogResult);
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _loadedInstances.Add(this);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            foreach (var item in _dialogs.ToArray())
            {
                Close(item, DialogResult.None);
            }
            _loadedInstances.Remove(this);
        }
    }
}