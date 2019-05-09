using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Behaviour for a marquee container.
    /// </summary>
    public class MarqueeContainer : Behavior<Panel>
    {
        /// <summary>
        /// Dependency property for <see cref="ScrollDuration"/>.
        /// </summary>
        public static readonly DependencyProperty ScrollDurationProperty
            = DependencyProperty.Register(nameof(ScrollDuration), typeof(TimeSpan), typeof(MarqueeContainer), new PropertyMetadata(TimeSpan.Zero, ScrollDurationPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="TargetChild"/>.
        /// </summary>
        public static readonly DependencyProperty TargetChildProperty
            = DependencyProperty.Register(nameof(TargetChild), typeof(FrameworkElement), typeof(MarqueeContainer), new PropertyMetadata(TargetChildPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="DuplicateChild"/>.
        /// </summary>
        public static readonly DependencyProperty DuplicateChildProperty
            = DependencyProperty.Register(nameof(DuplicateChild), typeof(FrameworkElement), typeof(MarqueeContainer));

        private const double ChildMargin = 100;

        /// <summary>
        /// Gets or sets the target child of the marquee animation.
        /// </summary>
        public FrameworkElement TargetChild
        {
            get => (FrameworkElement)GetValue(TargetChildProperty);
            set => SetValue(TargetChildProperty, value);
        }

        /// <summary>
        /// Gets or sets the duplicate child.
        /// </summary>
        public FrameworkElement DuplicateChild
        {
            get => (FrameworkElement)GetValue(DuplicateChildProperty);
            set => SetValue(DuplicateChildProperty, value);
        }

        /// <summary>
        /// Gets or sets the scroll duration for the child to scroll from one end to the other.
        /// </summary>
        public TimeSpan ScrollDuration
        {
            get => (TimeSpan)GetValue(ScrollDurationProperty);
            set => SetValue(ScrollDurationProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        private static void ScrollDurationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var marquee = (MarqueeContainer)d;
            marquee.UpdateAnimation();
        }

        private static void TargetChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var marque = (MarqueeContainer)d;

            if (e.OldValue != null)
            {
                ((FrameworkElement)e.OldValue).SizeChanged -= marque.TargetChildOnSizeChanged;
                marque.UpdateAnimation();
            }

            if (e.NewValue != null)
            {
                ((FrameworkElement)e.NewValue).SizeChanged += marque.TargetChildOnSizeChanged;
                marque.UpdateAnimation();
            }
        }

        private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateAnimation();
        }

        private void TargetChildOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (TargetChild == null)
            {
                return;
            }

            StopAnimation(TargetChild);
            StopAnimation(DuplicateChild);

            if (TargetChild.ActualWidth <= AssociatedObject.Width || ScrollDuration == TimeSpan.Zero)
            {
                if (DuplicateChild != null)
                {
                    DuplicateChild.Opacity = 0;
                }

                return;
            }

            StartAnimation(TargetChild);

            if (DuplicateChild != null)
            {
                DuplicateChild.Opacity = 1;
            }

            StartAnimation(DuplicateChild, TimeSpan.FromMilliseconds(ScrollDuration.TotalMilliseconds / 2));
            AssociatedObject.InvalidateVisual();
        }

        private void StartAnimation(FrameworkElement child, TimeSpan? offset = null)
        {
            if (child == null)
            {
                return;
            }

            var translateTransform = new TranslateTransform(TargetChild.ActualWidth, 0);
            child.RenderTransform = translateTransform;
            var animation = new DoubleAnimation
            {
                From = TargetChild.ActualWidth,
                To = -TargetChild.ActualWidth - ChildMargin,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(ScrollDuration),
            };

            if (offset.HasValue)
            {
                animation.BeginTime = offset.Value;
            }

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void StopAnimation(FrameworkElement child)
        {
            if (child == null)
            {
                return;
            }

            if (child.RenderTransform is TranslateTransform transform)
            {
                transform.BeginAnimation(TranslateTransform.XProperty, null);
            }

            child.RenderTransform = Transform.Identity;
        }
    }
}
