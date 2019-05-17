using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Provides a way to update the slide value only when the thumb drag is completed or a new position is clicked.
    /// </summary>
    public class SliderThrottle : Behavior<Slider>
    {
        /// <summary>
        /// Dependency property for <see cref="FinalValueProperty"/>.
        /// </summary>
        public static readonly DependencyProperty FinalValueProperty
            = DependencyProperty.Register(nameof(FinalValue), typeof(double), typeof(SliderThrottle), new PropertyMetadata(default(double), FinalValuePropertyChangedCallback));

        private bool _isMouseDown;

        /// <summary>
        /// Gets or sets the final value of the slider when changing.
        /// </summary>
        public double FinalValue
        {
            get => (double)GetValue(FinalValueProperty);
            set => SetValue(FinalValueProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseUp;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseUp;
        }

        private static void FinalValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SliderThrottle)d).ValueChanged();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            FinalValue = AssociatedObject.Value;
        }

        private void ValueChanged()
        {
            if (_isMouseDown)
            {
                return;
            }

            AssociatedObject.Value = FinalValue;
        }
    }
}
