using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AudioBand.UI
{
    /// <summary>
    /// Behaviour that exposes mouse coordinates.
    /// </summary>
    public class MouseTracker : Behavior<FrameworkElement>
    {
        /// <summary>
        /// The dependency property for <see cref="RelativeX"/>.
        /// </summary>
        public static readonly DependencyProperty RelativeXProperty
            = DependencyProperty.Register(nameof(RelativeX), typeof(double), typeof(MouseTracker), new PropertyMetadata(default(double)));

        /// <summary>
        /// The dependency property for <see cref="RelativeY"/>.
        /// </summary>
        public static readonly DependencyProperty RelativeYProperty
            = DependencyProperty.Register(nameof(RelativeY), typeof(double), typeof(MouseTracker), new PropertyMetadata(default(double)));

        /// <summary>
        /// The dependency property for <see cref="ScreenX"/>.
        /// </summary>
        public static readonly DependencyProperty ScreenXProperty
            = DependencyProperty.Register(nameof(ScreenX), typeof(double), typeof(MouseTracker), new PropertyMetadata(default(double)));

        /// <summary>
        /// The dependency property for <see cref="ScreenY"/>.
        /// </summary>
        public static readonly DependencyProperty ScreenYProperty
            = DependencyProperty.Register(nameof(ScreenY), typeof(double), typeof(MouseTracker), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the relative x position of the mouse.
        /// </summary>
        public double RelativeX
        {
            get => (double)GetValue(RelativeXProperty);
            set => SetValue(RelativeXProperty, value);
        }

        /// <summary>
        /// Gets or sets the relative x position of the mouse.
        /// </summary>
        public double RelativeY
        {
            get => (double)GetValue(RelativeYProperty);
            set => SetValue(RelativeYProperty, value);
        }

        /// <summary>
        /// Gets or sets the screen x position of the mouse.
        /// </summary>
        public double ScreenX
        {
            get => (double)GetValue(ScreenXProperty);
            set => SetValue(ScreenXProperty, value);
        }

        /// <summary>
        /// Gets or sets the screen y position of the mouse.
        /// </summary>
        public double ScreenY
        {
            get => (double)GetValue(ScreenYProperty);
            set => SetValue(ScreenYProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            RelativeX = pos.X;
            RelativeY = pos.Y;

            var screen = AssociatedObject.PointToScreen(pos);
            ScreenX = screen.X;
            ScreenY = screen.Y;
        }
    }
}
