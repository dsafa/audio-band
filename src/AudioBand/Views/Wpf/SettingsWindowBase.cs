using System;
using System.Windows;
using System.Windows.Media;
using AudioBand.Extensions;
using AudioBand.Messages;
using SourceChord.FluentWPF;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Base window class that helps with dpi scaling.
    /// </summary>
    public class SettingsWindowBase : AcrylicWindow
    {
        /// <summary>
        /// The dependency property for current dpi.
        /// </summary>
        public static readonly DependencyProperty CurrentDpiProperty
            = DependencyProperty.Register("CurrentDpi", typeof(double), typeof(SettingsWindowBase), new PropertyMetadata(96.0));

        private readonly IMessageBus _messageBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowBase"/> class.
        /// </summary>
        public SettingsWindowBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowBase"/> class.
        /// </summary>
        /// <param name="messageBus">The message bus.</param>
        public SettingsWindowBase(IMessageBus messageBus)
            : this()
        {
            _messageBus = messageBus;
            messageBus.Subscribe<DpiChangedMessage>(DpiChangedMessageHandler);
            Loaded += OnLoaded;
        }

        /// <inheritdoc />
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _messageBus.Unsubscribe<DpiChangedMessage>(DpiChangedMessageHandler);
        }

        private void DpiChangedMessageHandler(DpiChangedMessage msg)
        {
            UpdateDpi(msg.NewDpi);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateDpi(this.GetDpi());
        }

        private void UpdateDpi(double newDpi)
        {
            var child = GetVisualChild(0);
            if (child == null)
            {
                return;
            }

            var currentDpi = (double)child.GetValue(CurrentDpiProperty);
            var resizeFactor = newDpi / currentDpi;
            Width *= resizeFactor;
            Height *= resizeFactor;

            if (Math.Abs(newDpi - 96.0) < 0.001)
            {
                child.SetValue(LayoutTransformProperty, null);
            }
            else
            {
                var scaleTransformFactor = newDpi / 96.0;
                child.SetValue(LayoutTransformProperty, new ScaleTransform(scaleTransformFactor, scaleTransformFactor));
            }

            child.SetValue(CurrentDpiProperty, newDpi);
        }
    }
}
