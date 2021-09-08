using AudioBand.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AudioBand.UI
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
        /// Dependency property for <see cref="TargetCopy"/>.
        /// </summary>
        public static readonly DependencyProperty TargetCopyProperty
            = DependencyProperty.Register(nameof(TargetCopy), typeof(FrameworkElement), typeof(MarqueeContainer), new PropertyMetadata(TargetCopyPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="Models.TextOverflow"/>.
        /// </summary>
        public static readonly DependencyProperty TextOverflowProperty
            = DependencyProperty.Register(nameof(Models.TextOverflow), typeof(TextOverflow), typeof(MarqueeContainer), new PropertyMetadata(Models.TextOverflow.Truncate, TextOverflowPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="Models.ScrollBehavior"/>.
        /// </summary>
        public static readonly DependencyProperty ScrollBehaviorProperty
            = DependencyProperty.Register(nameof(Models.ScrollBehavior), typeof(ScrollBehavior), typeof(MarqueeContainer), new PropertyMetadata(ScrollBehavior.Always, ScrollBehaviorPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="IsPlaying"/>.
        /// </summary>
        public static readonly DependencyProperty IsPlayingProperty
            = DependencyProperty.Register(nameof(IsPlaying), typeof(bool), typeof(MarqueeContainer), new PropertyMetadata(false, IsPlayingPropertyChangedCallback));

        /// <summary>
        /// Attached dependency property for IsScrolling.
        /// </summary>
        public static readonly DependencyProperty IsScrollingProperty
            = DependencyProperty.RegisterAttached("IsScrolling", typeof(bool), typeof(MarqueeContainer), new PropertyMetadata(false));

        private const double ChildMargin = 50;
        private bool _mouseOver;
        private Storyboard _storyBoard;

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
        /// <remarks>Tried to use visual brush for copying but had alignment issues.</remarks>
        public FrameworkElement TargetCopy
        {
            get => (FrameworkElement)GetValue(TargetCopyProperty);
            set => SetValue(TargetCopyProperty, value);
        }

        /// <summary>
        /// Gets or sets the scroll duration for the child to scroll from one end to the other.
        /// </summary>
        public TimeSpan ScrollDuration
        {
            get => (TimeSpan)GetValue(ScrollDurationProperty);
            set => SetValue(ScrollDurationProperty, value);
        }

        /// <summary>
        /// Gets or sets the text overflow.
        /// </summary>
        public TextOverflow TextOverflow
        {
            get => (TextOverflow)GetValue(TextOverflowProperty);
            set => SetValue(TextOverflowProperty, value);
        }

        /// <summary>
        /// Gets or sets the scroll behavior.
        /// </summary>
        public ScrollBehavior ScrollBehavior
        {
            get => (ScrollBehavior)GetValue(ScrollBehaviorProperty);
            set => SetValue(ScrollBehaviorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether a track is playing.
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="IsScrollingProperty"/>.
        /// </summary>
        /// <param name="panel">The element to get the value from.</param>
        /// <returns>The value of the property.</returns>
        public static bool GetIsScrolling(Panel panel)
        {
            return (bool)panel.GetValue(IsScrollingProperty);
        }

        /// <summary>
        /// Sets the <see cref="IsScrollingProperty"/>.
        /// </summary>
        /// <param name="panel">The element to set the value on.</param>
        /// <param name="value">The value of the property.</param>
        public static void SetIsScrolling(Panel panel, bool value)
        {
            panel.SetValue(IsScrollingProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
            AssociatedObject.MouseEnter += AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseLeave += AssociatedObjectOnMouseLeave;
            SetIsScrolling(AssociatedObject, false);
        }

        private static void TargetChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var marquee = (MarqueeContainer)d;
            var targetChild = (FrameworkElement)e.NewValue;
            targetChild.RenderTransform = new TranslateTransform();
            targetChild.SizeChanged += marquee.TargetChildOnSizeChanged;
        }

        private static void TargetCopyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var copy = (FrameworkElement)e.NewValue;
            copy.Opacity = 0;
            copy.RenderTransform = new TranslateTransform();
        }

        private static void ScrollDurationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarqueeContainer)d).UpdateAnimation();
        }

        private static void ScrollBehaviorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarqueeContainer)d).UpdateAnimation();
        }

        private static void TextOverflowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarqueeContainer)d).UpdateAnimation();
        }

        private static void IsPlayingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = (MarqueeContainer)d;
            if (container.ScrollBehavior == ScrollBehavior.OnlyWhenTrackPlaying)
            {
                container.UpdateAnimation();
            }
        }

        private void AssociatedObjectOnMouseEnter(object sender, MouseEventArgs e)
        {
            _mouseOver = true;
            if (ScrollBehavior == ScrollBehavior.OnlyWhenMouseOver)
            {
                UpdateAnimation();
            }
        }

        private void AssociatedObjectOnMouseLeave(object sender, MouseEventArgs e)
        {
            _mouseOver = false;
            if (ScrollBehavior == ScrollBehavior.OnlyWhenMouseOver)
            {
                UpdateAnimation();
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
            if (TargetChild == null || TargetCopy == null)
            {
                return;
            }

            var shouldScroll = false;
            if (TextOverflow == TextOverflow.Scroll)
            {
                switch (ScrollBehavior)
                {
                    case ScrollBehavior.OnlyWhenMouseOver when _mouseOver:
                    case ScrollBehavior.OnlyWhenTrackPlaying when IsPlaying:
                    case ScrollBehavior.Always:
                        shouldScroll = true;
                        break;
                }
            }

            if (TargetChild.ActualWidth <= AssociatedObject.Width || ScrollDuration == TimeSpan.Zero || !shouldScroll)
            {
                StopAnimation();
                return;
            }

            StartAnimation();
            AssociatedObject.InvalidateVisual();
        }

        private void StopAnimation()
        {
            _storyBoard?.Stop();
            TargetCopy.Opacity = 0;
            TargetCopy.RenderTransform.BeginAnimation(TranslateTransform.XProperty, null);
            SetIsScrolling(AssociatedObject, false);
        }

        private void StartAnimation()
        {
            // Create animation to scroll from current to left, then a second animation that scrolls from right to left forever
            var firstAnimation = new DoubleAnimation
            {
                From = 0,
                By = -TargetChild.ActualWidth - ChildMargin,
                Duration = CalculateTimeToScroll(TargetChild.ActualWidth + ChildMargin),
            };

            Storyboard.SetTarget(firstAnimation, TargetChild);
            Storyboard.SetTargetProperty(firstAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            var secondAnimation = CreateFullScrollAnimation();
            secondAnimation.BeginTime = firstAnimation.Duration.TimeSpan;

            Storyboard.SetTarget(secondAnimation, TargetChild);
            Storyboard.SetTargetProperty(secondAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            _storyBoard = new Storyboard();
            _storyBoard.Children.Add(firstAnimation);
            _storyBoard.Children.Add(secondAnimation);

            var copyAnimation = CreateFullScrollAnimation();
            TargetCopy.Opacity = 1;
            TargetCopy.RenderTransform = new TranslateTransform(TargetChild.ActualWidth + ChildMargin, 0);
            TargetChild.RenderTransform = new TranslateTransform();

            _storyBoard.Begin();
            TargetCopy.RenderTransform.BeginAnimation(TranslateTransform.XProperty, copyAnimation);
            SetIsScrolling(AssociatedObject, true);
        }

        private DoubleAnimation CreateFullScrollAnimation()
        {
            return new DoubleAnimation
            {
                From = TargetChild.ActualWidth + ChildMargin,
                By = -2 * (TargetChild.ActualWidth + ChildMargin),
                Duration = CalculateTimeToFullyScroll(),
                RepeatBehavior = RepeatBehavior.Forever,
            };
        }

        private TimeSpan CalculateTimeToScroll(double scrollDistance)
        {
            // Calculate time to scroll based on the target time to scroll across the panel.
            if (ScrollDuration.TotalMilliseconds == 0)
            {
                return TimeSpan.Zero;
            }

            var velocity = AssociatedObject.ActualWidth / ScrollDuration.TotalMilliseconds;
            var scrollTime = scrollDistance / velocity;

            return TimeSpan.FromMilliseconds(scrollTime);
        }

        private TimeSpan CalculateTimeToFullyScroll()
        {
            return CalculateTimeToScroll(2 * (TargetChild.ActualWidth + ChildMargin));
        }
    }
}
