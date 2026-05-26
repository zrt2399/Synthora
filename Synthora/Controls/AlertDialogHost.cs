using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Media;
using Synthora.Overlays;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a single alert dialog instance currently managed by an <see cref="AlertDialogHost"/>.
    /// </summary>
    public class AlertDialogInstance
    {
        public string? Title { get; }
        public string? Message { get; }
        public DialogButton DialogButton { get; }
        public IconType IconType { get; }
        public bool ShowCloseButton { get; }
        public double DialogMaxWidth { get; }

        public AlertDialogHost Host { get; }
        public TaskCompletionSource<DialogResult> Tcs { get; } = new();

        /// <summary>
        /// Initializes a dialog instance from the supplied host and dialog arguments.
        /// </summary>
        public AlertDialogInstance(AlertDialogHost host, AlertDialogArguments args)
        {
            Host = host;
            Title = args.Title;
            Message = args.Message;
            DialogButton = args.DialogButton;
            IconType = args.IconType;
            ShowCloseButton = args.ShowCloseButton || DialogButton == DialogButton.None;
            DialogMaxWidth = args.DialogMaxWidth;
        }

        public void OK() => Host.Close(this, DialogResult.OK);
        public void Cancel() => Host.Close(this, DialogResult.Cancel);
        public void Yes() => Host.Close(this, DialogResult.Yes);
        public void No() => Host.Close(this, DialogResult.No);
        public void Abort() => Host.Close(this, DialogResult.Abort);
        public void None() => Host.Close(this, DialogResult.None);
    }

    [PseudoClasses(pcOpen)]
    public class AlertDialogHost : ContentControl
    {
        private bool _isOpen;
        private const string pcOpen = ":open";
        private static readonly HashSet<AlertDialogHost> _loadedInstances = [];
        private readonly ObservableCollection<AlertDialogInstance> _dialogs = [];

        public static readonly StyledProperty<double> BlurRadiusProperty =
            AvaloniaProperty.Register<AlertDialogHost, double>(nameof(BlurRadius));

        public static readonly StyledProperty<string?> IdentifierProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Identifier));

        public static readonly StyledProperty<IBrush?> OverlayBackgroundProperty =
            AvaloniaProperty.Register<AlertDialogHost, IBrush?>(nameof(OverlayBackground));

        public static readonly StyledProperty<BoxShadows> DialogBoxShadowProperty =
            AvaloniaProperty.Register<AlertDialogHost, BoxShadows>(nameof(DialogBoxShadow));

        public static readonly StyledProperty<bool> DisableOpeningAnimationProperty =
            AvaloniaProperty.Register<AlertDialogHost, bool>(nameof(DisableOpeningAnimation));

        public static readonly DirectProperty<AlertDialogHost, bool> IsOpenProperty =
            AvaloniaProperty.RegisterDirect<AlertDialogHost, bool>(nameof(IsOpen), o => o.IsOpen);

        public IReadOnlyList<AlertDialogInstance> Dialogs => _dialogs;

        public double BlurRadius
        {
            get => GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        public string? Identifier
        {
            get => GetValue(IdentifierProperty);
            set => SetValue(IdentifierProperty, value);
        }

        public IBrush? OverlayBackground
        {
            get => GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        public BoxShadows DialogBoxShadow
        {
            get => GetValue(DialogBoxShadowProperty);
            set => SetValue(DialogBoxShadowProperty, value);
        }

        public bool DisableOpeningAnimation
        {
            get => GetValue(DisableOpeningAnimationProperty);
            set => SetValue(DisableOpeningAnimationProperty, value);
        }

        public bool IsOpen
        {
            get => _isOpen;
            internal set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

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

        internal async Task<DialogResult> Show(AlertDialogArguments alertDialogArguments)
        {
            var dialog = new AlertDialogInstance(this, alertDialogArguments);
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