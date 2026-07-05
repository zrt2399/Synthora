using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Synthora.Attaches;

namespace Synthora.Controls
{
    /// <summary>
    /// Presents a control header with an optional required indicator.
    /// </summary>
    public class HeaderPresenter : ContentControl
    {
        private readonly List<IDisposable> _subscriptions = [];
        private Control? _subscriptionParent;

        /// <summary>
        /// Defines the <see cref="IsRequired"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsRequiredProperty =
            AvaloniaProperty.Register<HeaderPresenter, bool>(nameof(IsRequired));

        /// <summary>
        /// Gets or sets whether the required indicator is shown.
        /// </summary>
        public bool IsRequired
        {
            get => GetValue(IsRequiredProperty);
            set => SetValue(IsRequiredProperty, value);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UpdateTemplatedParentBindings();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            ClearSubscriptions();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == TemplatedParentProperty)
            {
                UpdateTemplatedParentBindings();
            }
        }

        private void UpdateTemplatedParentBindings()
        {
            var templatedParent = TemplatedParent as Control;
            if (ReferenceEquals(_subscriptionParent, templatedParent))
            {
                return;
            }

            ClearSubscriptions();
            _subscriptionParent = templatedParent;

            if (templatedParent is null)
            {
                return;
            }

            _subscriptions.Add(Bind(ContentProperty, templatedParent.GetObservable(TextBoxAttach.HeaderProperty)));
            _subscriptions.Add(Bind(HorizontalAlignmentProperty, templatedParent.GetObservable(TextBoxAttach.HeaderHorizontalAlignmentProperty)));
            _subscriptions.Add(Bind(VerticalAlignmentProperty, templatedParent.GetObservable(TextBoxAttach.HeaderVerticalAlignmentProperty)));
            _subscriptions.Add(Bind(DockPanel.DockProperty, templatedParent.GetObservable(TextBoxAttach.HeaderPlacementProperty)));
            _subscriptions.Add(Bind(IsRequiredProperty, templatedParent.GetObservable(TextBoxAttach.IsRequiredProperty)));
        }

        private void ClearSubscriptions()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            _subscriptions.Clear();
            _subscriptionParent = null;
        }
    }
}