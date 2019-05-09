using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Behavior to allow slider to be dragged from anywhere.
    /// https://stackoverflow.com/questions/2909862/slider-does-not-drag-in-combination-with-ismovetopointenabled-behaviour.
    /// </summary>
    public sealed class SliderClickAndDrag : Behavior<Slider>
    {
        private Thumb _thumb;

        /// <inheritdoc />
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            _thumb = ((Track)AssociatedObject.Template.FindName("PART_Track", AssociatedObject)).Thumb;
            AssociatedObject.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Released || _thumb.IsDragging || !_thumb.IsMouseOver)
            {
                return;
            }

            _thumb.RaiseEvent(new MouseButtonEventArgs(args.MouseDevice, args.Timestamp, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent,
            });
        }
    }
}
