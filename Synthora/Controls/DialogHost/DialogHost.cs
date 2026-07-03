using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Synthora.Events;
using Synthora.Input;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a single dialog content item currently managed by a <see cref="DialogHost"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DialogHostInstance"/> class.
    /// </remarks>
    public class DialogHostInstance(DialogHost host, object? content)
    {
        /// <summary>
        /// Gets the host that owns this dialog.
        /// </summary>
        public DialogHost Host { get; } = host;

        /// <summary>
        /// Gets the dialog content.
        /// </summary>
        public object? Content { get; } = content;

        /// <summary>
        /// Gets the completion source used to publish the dialog close parameter.
        /// </summary>
        internal TaskCompletionSource<object?> Tcs { get; } = new TaskCompletionSource<object?>();

        /// <summary>
        /// Gets the horizontal alignment applied to the popup host for this dialog.
        /// </summary>
        public virtual HorizontalAlignment HorizontalAlignment => Host.HorizontalDialogAlignment;

        /// <summary>
        /// Gets the vertical alignment applied to the popup host for this dialog.
        /// </summary>
        public virtual VerticalAlignment VerticalAlignment => Host.VerticalDialogAlignment;

        /// <summary>
        /// Closes this dialog with the supplied close parameter.
        /// </summary>
        public void Close(object? parameter = null) => Host.CloseDialog(this, parameter);

        internal virtual void SetResult(object? parameter) => Tcs.TrySetResult(parameter);
    }

    /// <summary>
    /// Hosts dialog content and manages overlay state.
    /// </summary>
    [PseudoClasses(pcOpen)]
    public class DialogHost : ContentControl
    {
        private bool _isOpen;
        private const string pcOpen = ":open";
        private static readonly HashSet<DialogHost> _loadedInstances = [];
        private readonly ObservableCollection<DialogOverlayPopupHost> _dialogs = [];
        private readonly ICommand _openDialogCommand = null!;
        private readonly ICommand _closeDialogCommand = null!;

        /// <summary>
        /// Defines the <see cref="BlurRadius"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BlurRadiusProperty =
            AvaloniaProperty.Register<DialogHost, double>(nameof(BlurRadius));

        /// <summary>
        /// Defines the <see cref="Identifier"/> property.
        /// </summary>
        public static readonly StyledProperty<string?> IdentifierProperty =
            AvaloniaProperty.Register<DialogHost, string?>(nameof(Identifier));

        /// <summary>
        /// Defines the <see cref="OverlayBackground"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush?> OverlayBackgroundProperty =
            AvaloniaProperty.Register<DialogHost, IBrush?>(nameof(OverlayBackground));

        /// <summary>
        /// Defines the <see cref="DialogBoxShadow"/> property.
        /// </summary>
        public static readonly StyledProperty<BoxShadows> DialogBoxShadowProperty =
            AvaloniaProperty.Register<DialogHost, BoxShadows>(nameof(DialogBoxShadow));

        /// <summary>
        /// Defines the <see cref="DisableOpeningAnimation"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> DisableOpeningAnimationProperty =
            AvaloniaProperty.Register<DialogHost, bool>(nameof(DisableOpeningAnimation));

        /// <summary>
        /// Defines the <see cref="HorizontalDialogAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> HorizontalDialogAlignmentProperty =
            AvaloniaProperty.Register<DialogHost, HorizontalAlignment>(nameof(HorizontalDialogAlignment));

        /// <summary>
        /// Defines the <see cref="VerticalDialogAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> VerticalDialogAlignmentProperty =
            AvaloniaProperty.Register<DialogHost, VerticalAlignment>(nameof(VerticalDialogAlignment));

        /// <summary>
        /// Defines the <see cref="DialogOpened"/> routed event.
        /// </summary>
        public static readonly RoutedEvent<DialogOpenedEventArgs> DialogOpenedEvent =
            RoutedEvent.Register<DialogHost, DialogOpenedEventArgs>(nameof(DialogOpened), RoutingStrategies.Bubble);

        /// <summary>
        /// Defines the <see cref="DialogClosing"/> routed event.
        /// </summary>
        public static readonly RoutedEvent<DialogClosingEventArgs> DialogClosingEvent =
            RoutedEvent.Register<DialogHost, DialogClosingEventArgs>(nameof(DialogClosing), RoutingStrategies.Bubble);

        /// <summary>
        /// Defines the <see cref="DialogOpenedCallback"/> property.
        /// </summary>
        public static readonly StyledProperty<EventHandler<DialogOpenedEventArgs>?> DialogOpenedCallbackProperty =
            AvaloniaProperty.Register<DialogHost, EventHandler<DialogOpenedEventArgs>?>(nameof(DialogOpenedCallback));

        /// <summary>
        /// Defines the <see cref="DialogClosingCallback"/> property.
        /// </summary>
        public static readonly StyledProperty<EventHandler<DialogClosingEventArgs>?> DialogClosingCallbackProperty =
            AvaloniaProperty.Register<DialogHost, EventHandler<DialogClosingEventArgs>?>(nameof(DialogClosingCallback));

        /// <summary>
        /// Defines the <see cref="PopupTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IControlTemplate?> PopupTemplateProperty =
            AvaloniaProperty.Register<DialogHost, IControlTemplate?>(nameof(PopupTemplate));

        /// <summary>
        /// Defines the <see cref="DialogContent"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> DialogContentProperty =
            AvaloniaProperty.Register<DialogHost, object?>(nameof(DialogContent));

        /// <summary>
        /// Defines the <see cref="DialogContentTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate?> DialogContentTemplateProperty =
            AvaloniaProperty.Register<DialogHost, IDataTemplate?>(nameof(DialogContentTemplate));

        /// <summary>
        /// Defines the <see cref="IsOpen"/> property.
        /// </summary>
        public static readonly DirectProperty<DialogHost, bool> IsOpenProperty =
            AvaloniaProperty.RegisterDirect<DialogHost, bool>(nameof(IsOpen), o => o.IsOpen);

        /// <summary>
        /// Defines the <see cref="OpenDialogCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<DialogHost, ICommand> OpenDialogCommandProperty =
            AvaloniaProperty.RegisterDirect<DialogHost, ICommand>(nameof(OpenDialogCommand), o => o.OpenDialogCommand);

        /// <summary>
        /// Defines the <see cref="CloseDialogCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<DialogHost, ICommand> CloseDialogCommandProperty =
            AvaloniaProperty.RegisterDirect<DialogHost, ICommand>(nameof(CloseDialogCommand), o => o.CloseDialogCommand);

        /// <summary>
        /// Gets the dialogs currently displayed by the host.
        /// </summary>
        public IReadOnlyList<DialogOverlayPopupHost> Dialogs => _dialogs;

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
        /// Gets or sets the horizontal alignment applied to each dialog popup host.
        /// </summary>
        public HorizontalAlignment HorizontalDialogAlignment
        {
            get => GetValue(HorizontalDialogAlignmentProperty);
            set => SetValue(HorizontalDialogAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertical alignment applied to each dialog popup host.
        /// </summary>
        public VerticalAlignment VerticalDialogAlignment
        {
            get => GetValue(VerticalDialogAlignmentProperty);
            set => SetValue(VerticalDialogAlignmentProperty, value);
        }

        /// <summary>
        /// Raised when a dialog is opened.
        /// </summary>
        public event EventHandler<DialogOpenedEventArgs> DialogOpened
        {
            add => AddHandler(DialogOpenedEvent, value);
            remove => RemoveHandler(DialogOpenedEvent, value);
        }

        /// <summary>
        /// Raised just before a dialog is closed.
        /// </summary>
        public event EventHandler<DialogClosingEventArgs> DialogClosing
        {
            add => AddHandler(DialogClosingEvent, value);
            remove => RemoveHandler(DialogClosingEvent, value);
        }

        /// <summary>
        /// Gets or sets the callback invoked after a dialog is opened.
        /// </summary>
        public EventHandler<DialogOpenedEventArgs>? DialogOpenedCallback
        {
            get => GetValue(DialogOpenedCallbackProperty);
            set => SetValue(DialogOpenedCallbackProperty, value);
        }

        /// <summary>
        /// Gets or sets the callback invoked before a dialog is closed.
        /// </summary>
        public EventHandler<DialogClosingEventArgs>? DialogClosingCallback
        {
            get => GetValue(DialogClosingCallbackProperty);
            set => SetValue(DialogClosingCallbackProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to render each popup dialog instance.
        /// </summary>
        public IControlTemplate? PopupTemplate
        {
            get => GetValue(PopupTemplateProperty);
            set => SetValue(PopupTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the default dialog content used by <see cref="OpenDialogCommand"/>.
        /// </summary>
        public object? DialogContent
        {
            get => GetValue(DialogContentProperty);
            set => SetValue(DialogContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to render dialog content.
        /// </summary>
        public IDataTemplate? DialogContentTemplate
        {
            get => GetValue(DialogContentTemplateProperty);
            set => SetValue(DialogContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the host currently has an open dialog.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            protected set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

        /// <summary>
        /// Gets the command to open the dialog.
        /// </summary>
        public ICommand OpenDialogCommand
        {
            get => _openDialogCommand;
            private init => SetAndRaise(OpenDialogCommandProperty, ref _openDialogCommand, value);
        }

        /// <summary>
        /// Gets the command to close the dialog.
        /// </summary>
        public ICommand CloseDialogCommand
        {
            get => _closeDialogCommand;
            private init => SetAndRaise(CloseDialogCommandProperty, ref _closeDialogCommand, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogHost"/> class.
        /// </summary>
        public DialogHost()
        {
            OpenDialogCommand = new RelayCommand<object?>(ExecuteOpenDialogCommand, _ => !IsOpen);
            CloseDialogCommand = new RelayCommand<object?>(ExecuteCloseDialogCommand, _ => IsOpen);

            _dialogs.CollectionChanged += (_, _) =>
            {
                IsOpen = _dialogs.Count > 0;
                PseudoClasses.Set(pcOpen, IsOpen);
                (OpenDialogCommand as RelayCommand<object?>)?.NotifyCanExecuteChanged();
                (CloseDialogCommand as RelayCommand<object?>)?.NotifyCanExecuteChanged();
            };
        }

        /// <summary>
        /// Finds a loaded dialog host by identifier.
        /// </summary>
        public static DialogHost GetInstance(string? dialogIdentifier) => GetInstance<DialogHost>(dialogIdentifier);

        /// <summary>
        /// Shows content on the dialog host identified by <paramref name="dialogIdentifier"/>.
        /// </summary>
        public static Task<object?> Show(object? content, string? dialogIdentifier = null)
        {
            return GetInstance(dialogIdentifier).ShowDialog(content);
        }

        /// <summary>
        /// Closes a dialog on the host identified by <paramref name="dialogIdentifier"/>.
        /// </summary>
        public static void Close(object? parameter, string? dialogIdentifier = null, object? content = null)
        {
            GetInstance(dialogIdentifier).CloseDialog(parameter, content);
        }

        /// <summary>
        /// Shows dialog content.
        /// </summary>
        public Task<object?> ShowDialog(object? content)
        {
            if (CheckAccess())
            {
                var dialogHostInstance = new DialogHostInstance(this, content);
                OpenDialog(dialogHostInstance);

                return dialogHostInstance.Tcs.Task;
            }
            return Dispatcher.Invoke(() => ShowDialog(content));
        }

        /// <summary>
        /// Closes a dialog.
        /// </summary>
        public void CloseDialog(object? parameter, object? content = null)
        {
            if (CheckAccess())
            {
                var dialog = ResolveDialog(content);
                if (dialog != null)
                {
                    CloseDialog(dialog, parameter);
                }
            }
            else
            {
                Dispatcher.Post(() => CloseDialog(parameter, content));
            }
        }

        internal static T GetInstance<T>(string? dialogIdentifier) where T : DialogHost
        {
            if (_loadedInstances.Count == 0)
            {
                throw new InvalidOperationException($"No loaded {typeof(T).Name} instances.");
            }
            
            var targets = _loadedInstances.OfType<T>().Where(x => x.Identifier == dialogIdentifier).ToList();
            if (targets.Count == 0)
            {
                throw new InvalidOperationException($"No loaded {typeof(T).Name} have an {nameof(Identifier)} property matching {nameof(dialogIdentifier)} ('{dialogIdentifier}') argument.");
            }

            if (targets.Count > 1)
            {
                throw new InvalidOperationException($"Multiple viable {typeof(T).Name}s. Specify a unique Identifier on each DialogHost, especially where multiple Windows are a concern.");
            }

            return targets[0];
        }

        internal void OpenDialog(DialogHostInstance dialogHostInstance)
        {
            var dialogOverlayPopupHost = new DialogOverlayPopupHost
            {
                DialogHostInstance = dialogHostInstance,
                Content = dialogHostInstance.Content ?? DialogContent,
                ContentTemplate = DialogContentTemplate,
                DisableOpeningAnimation = DisableOpeningAnimation,
                HorizontalAlignment = dialogHostInstance.HorizontalAlignment,
                VerticalAlignment = dialogHostInstance.VerticalAlignment,
                Template = PopupTemplate
            };

            Dispatcher.InvokeAsync(() =>
            {
                dialogOverlayPopupHost.Focus();
            }, DispatcherPriority.Loaded);

            _dialogs.Add(dialogOverlayPopupHost);
            dialogOverlayPopupHost.IsOpen = true;

            RaiseDialogOpenedEvent(dialogHostInstance);
        }

        internal void CloseDialog(DialogHostInstance dialogInstance, object? parameter, bool canCancel = true)
        {
            var dialogOverlayPopupHost = _dialogs.FirstOrDefault(x => x.DialogHostInstance == dialogInstance);
            if (dialogOverlayPopupHost == null)
            {
                return;
            }

            if (RaiseDialogClosingEvent(dialogInstance, parameter) && canCancel)
            {
                return;
            }

            _dialogs.Remove(dialogOverlayPopupHost);
            dialogOverlayPopupHost.IsOpen = false;
            dialogOverlayPopupHost.ClearDialogReferences();
            dialogInstance.SetResult(parameter);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _loadedInstances.Add(this);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            foreach (var item in _dialogs.Select(x => x.DialogHostInstance).ToArray())
            {
                if (item != null)
                {
                    CloseDialog(item, null, canCancel: false);
                }
            }
            _loadedInstances.Remove(this);
        }

        private void RaiseDialogOpenedEvent(DialogHostInstance dialogInstance)
        {
            var eventArgs = new DialogOpenedEventArgs(dialogInstance)
            {
                RoutedEvent = DialogOpenedEvent,
                Source = this
            };

            RaiseEvent(eventArgs);
            DialogOpenedCallback?.Invoke(this, eventArgs);
        }

        private bool RaiseDialogClosingEvent(DialogHostInstance dialogInstance, object? parameter)
        {
            var eventArgs = new DialogClosingEventArgs(dialogInstance, parameter)
            {
                RoutedEvent = DialogClosingEvent,
                Source = this
            };

            RaiseEvent(eventArgs);
            DialogClosingCallback?.Invoke(this, eventArgs);

            return eventArgs.Cancel;
        }

        private DialogHostInstance? ResolveDialog(object? content)
        {
            var dialogHostInstance = _dialogs.LastOrDefault(x => x.DialogHostInstance?.Content == content)?.DialogHostInstance;
            return dialogHostInstance ?? _dialogs.LastOrDefault()?.DialogHostInstance;
        }
 
        private async void ExecuteOpenDialogCommand(object? parameter)
        {
            await ShowDialog(parameter);
        }

        private void ExecuteCloseDialogCommand(object? parameter)
        {
            CloseDialog(parameter);
        }
    }
}