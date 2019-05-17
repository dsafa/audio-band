using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Redirects mouse wheel events.
    /// </summary>
    public class RedirectScrolling : Behavior<FrameworkElement>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObjectOnPreviewMouseWheel;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObjectOnPreviewMouseWheel;
            base.OnDetaching();
        }

        private void AssociatedObjectOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var newEvent = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
            };

            AssociatedObject.RaiseEvent(newEvent);
        }
    }
}
