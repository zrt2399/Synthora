using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using Synthora.Messaging;

namespace Synthora.Controls
{
    public class AlertDialogClosedEventArgs(DialogResult dialogResult) : EventArgs
    {
        public DialogResult DialogResult { get; } = dialogResult;
    }

    public class AlertDialogParameter
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DialogButton DialogButton { get; set; } = DialogButton.OK;
        public IconType IconType { get; set; }
    }

    public class AlertDialogHost : DropShadowChrome
    {
        private StackPanel? PART_ButtonPanel;
        private static readonly HashSet<AlertDialogHost> _loadedInstances = [];

        public static readonly StyledProperty<double> BlurRadiusProperty =
            AvaloniaProperty.Register<AlertDialogHost, double>(nameof(BlurRadius), 1d);

        public static readonly StyledProperty<string?> TitleProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Title));

        public static readonly StyledProperty<string?> MessageProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Message));

        public static readonly StyledProperty<DialogButton> DialogButtonProperty =
            AvaloniaProperty.Register<AlertDialogHost, DialogButton>(nameof(DialogButton));

        public static readonly StyledProperty<IconType> IconTypeProperty =
            AvaloniaProperty.Register<AlertDialogHost, IconType>(nameof(IconType));

        public static readonly StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<AlertDialogHost, bool>(nameof(IsOpen));

        public static readonly StyledProperty<string?> IdentifierProperty =
            AvaloniaProperty.Register<AlertDialogHost, string?>(nameof(Identifier));

        public static readonly StyledProperty<IBrush> OverlayBackgroundProperty =
            AvaloniaProperty.Register<AlertDialogHost, IBrush>(nameof(OverlayBackground));

        public event EventHandler<AlertDialogClosedEventArgs>? DialogClosed;

        public double BlurRadius
        {
            get => GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string? Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public DialogButton DialogButton
        {
            get => GetValue(DialogButtonProperty);
            set => SetValue(DialogButtonProperty, value);
        }

        public IconType IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        public bool IsOpen
        {
            get => GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public string? Identifier
        {
            get => GetValue(IdentifierProperty);
            set => SetValue(IdentifierProperty, value);
        }

        public IBrush OverlayBackground
        {
            get => GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        private static AlertDialogHost GetInstance(string? dialogIdentifier)
        {
            if (_loadedInstances.Count == 0)
            {
                throw new InvalidOperationException("No loaded AlertDialogHost instances.");
            }

            var targets = _loadedInstances.Where(x => dialogIdentifier == null || Equals(x.Identifier, dialogIdentifier)).ToList();
            if (targets.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No loaded AlertDialogHost have an {nameof(Identifier)} property matching {nameof(dialogIdentifier)} ('{dialogIdentifier}') argument.");
            }

            if (targets.Count > 1)
            {
                throw new InvalidOperationException(
                    "Multiple viable AlertDialogHosts. Specify a unique Identifier on each AlertDialogHost, especially where multiple Windows are a concern.");
            }

            return targets[0];
        }

        public static bool IsDialogOpen(string? dialogIdentifier) => GetInstance(dialogIdentifier).IsOpen;

        internal static async Task<DialogResult> ShowAsync(string? dialogIdentifier, AlertDialogParameter alertDialogParameter)
        {
            return await GetInstance(dialogIdentifier).ShowCore(alertDialogParameter);
        }

        internal static async Task<DialogResult> ShowAsync(string? dialogIdentifier, string message, string title, DialogButton dialogButton, IconType iconType)
        {
            return await GetInstance(dialogIdentifier).ShowCore(new AlertDialogParameter()
            {
                Title = title,
                Message = message,
                DialogButton = dialogButton,
                IconType = iconType
            });
        }

        private async Task<DialogResult> ShowCore(AlertDialogParameter alertDialogParameter)
        {
            if (alertDialogParameter.DialogButton == DialogButton.None)
            {
                throw new InvalidOperationException("DialogButton cannot be None.");
            }

            if (IsOpen)
            {
                throw new InvalidOperationException("AlertDialogHost is already open.");
            }

            var tcs = new TaskCompletionSource<DialogResult>();

            Title = alertDialogParameter.Title;
            Message = alertDialogParameter.Message;
            DialogButton = alertDialogParameter.DialogButton;
            IconType = alertDialogParameter.IconType;
            IsOpen = true;
            DialogClosed += OnDialogClosed;

            _ = Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (PART_ButtonPanel != null && PART_ButtonPanel.Children.FirstOrDefault(x => x is Button && x.IsVisible) is Button button)
                {
                    button.Focus();
                }
            }, DispatcherPriority.Render);

            var dialogResult = await tcs.Task;
            DialogClosed -= OnDialogClosed;
            return dialogResult;

            void OnDialogClosed(object? sender, AlertDialogClosedEventArgs e)
            {
                IsOpen = false;
                tcs.TrySetResult(e.DialogResult);
            }
        }

        private void Close(DialogResult dialogResult)
        {
            DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(dialogResult));
        }

        public static void Close(string? dialogIdentifier, DialogResult dialogResult)
        {
            var dialogHost = GetInstance(dialogIdentifier);
            if (dialogHost != null)
            {
                dialogHost.Close(dialogResult);
                return;
            }

            throw new InvalidOperationException("AlertDialogHost is not open.");
        }

        /// <inheritdoc />
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _loadedInstances.Add(this);
        }

        /// <inheritdoc />
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _loadedInstances.Remove(this);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PART_ButtonPanel = e.NameScope.Find<StackPanel>(nameof(PART_ButtonPanel));
        }

        public void OK() => DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(DialogResult.OK));
        public void Cancel() => DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(DialogResult.Cancel));
        public void Yes() => DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(DialogResult.Yes));
        public void No() => DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(DialogResult.No));
        public void Abort() => DialogClosed?.Invoke(this, new AlertDialogClosedEventArgs(DialogResult.Abort));
    }
}