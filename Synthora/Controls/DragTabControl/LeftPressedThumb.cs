using System;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Synthora.Controls
{
    /// <summary>
    /// Represents a thumb that starts dragging only from left pointer-button input.
    /// </summary>
    public class LeftPressedThumb : TemplatedControl
    {
        /// <summary>
        /// Identifies the <see cref="DragStarted"/> routed event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragStartedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>(nameof(DragStarted), RoutingStrategies.Bubble);

        /// <summary>
        /// Identifies the <see cref="DragDelta"/> routed event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragDeltaEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>(nameof(DragDelta), RoutingStrategies.Bubble);

        /// <summary>
        /// Identifies the <see cref="DragCompleted"/> routed event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragCompletedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>(nameof(DragCompleted), RoutingStrategies.Bubble);

        private Point? _lastPoint;

        /// <summary>
        /// Occurs when a left-button drag operation starts.
        /// </summary>
        public event EventHandler<VectorEventArgs>? DragStarted
        {
            add => AddHandler(DragStartedEvent, value);
            remove => RemoveHandler(DragStartedEvent, value);
        }

        /// <summary>
        /// Occurs when a left-button drag operation moves.
        /// </summary>
        public event EventHandler<VectorEventArgs>? DragDelta
        {
            add => AddHandler(DragDeltaEvent, value);
            remove => RemoveHandler(DragDeltaEvent, value);
        }

        /// <summary>
        /// Occurs when a left-button drag operation completes.
        /// </summary>
        public event EventHandler<VectorEventArgs>? DragCompleted
        {
            add => AddHandler(DragCompletedEvent, value);
            remove => RemoveHandler(DragCompletedEvent, value);
        }

        protected override AutomationPeer OnCreateAutomationPeer() => new LeftPressedThumbPeer(this);

        protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragCompletedEvent,
                    Vector = _lastPoint.Value,
                };

                _lastPoint = null;

                RaiseEvent(ev);
            }

            base.OnPointerCaptureLost(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (!IsLeftButtonPressed(e))
            {
                return;
            }

            if (_lastPoint.HasValue)
            {
                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragDeltaEvent,
                    Vector = e.GetPosition(this) - _lastPoint.Value,
                };

                RaiseEvent(ev);
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (!IsLeftButtonPressed(e))
            {
                return;
            }

            e.Handled = true;
            _lastPoint = e.GetPosition(this);

            var ev = new VectorEventArgs
            {
                RoutedEvent = DragStartedEvent,
                Vector = (Point)_lastPoint,
            };

            e.PreventGestureRecognition();

            RaiseEvent(ev);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (!IsLeftButtonPressed(e))
            {
                return;
            }

            if (_lastPoint.HasValue)
            {
                e.Handled = true;
                _lastPoint = null;

                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragCompletedEvent,
                    Vector = e.GetPosition(this),
                };

                RaiseEvent(ev);
            }
        }

        private bool IsLeftButtonPressed(PointerEventArgs args)
        {
            var point = args.GetCurrentPoint(this);

            return point.Properties.IsLeftButtonPressed;
        }

        private class LeftPressedThumbPeer(LeftPressedThumb owner) : ControlAutomationPeer(owner)
        {
            protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Thumb;
            protected override bool IsContentElementCore() => false;
        }
    }
}