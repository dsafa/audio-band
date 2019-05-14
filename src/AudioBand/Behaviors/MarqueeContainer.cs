using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AudioBand.Models;

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
            = DependencyProperty.Register(nameof(DuplicateChild), typeof(FrameworkElement), typeof(MarqueeContainer), new PropertyMetadata(DuplicateChildPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="TextOverflow"/>.
        /// </summary>
        public static readonly DependencyProperty TextOverflowProperty
            = DependencyProperty.Register(nameof(TextOverflow), typeof(TextOverflow), typeof(MarqueeContainer), new PropertyMetadata(Models.TextOverflow.Truncate, TextOverflowPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="ScrollBehavior"/>.
        /// </summary>
        public static readonly DependencyProperty ScrollBehaviorProperty
            = DependencyProperty.Register(nameof(ScrollBehavior), typeof(ScrollBehavior), typeof(MarqueeContainer), new PropertyMetadata(ScrollBehavior.Always, ScrollBehaviorPropertyChangedCallback));

        /// <summary>
        /// Dependency property for <see cref="IsPlaying"/>.
        /// </summary>
        public static readonly DependencyProperty IsPlayingProperty
            = DependencyProperty.Register(nameof(IsPlaying), typeof(bool), typeof(MarqueeContainer), new PropertyMetadata(false, IsPlayingPropertyChangedCallback));

        private const double ChildMargin = 100;
        private bool _mouseOver;

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

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
            AssociatedObject.MouseEnter += AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseLeave += AssociatedObjectOnMouseLeave;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
            AssociatedObject.MouseEnter -= AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseLeave -= AssociatedObjectOnMouseLeave;
        }

        private static void TargetChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var marquee = (MarqueeContainer)d;
            ((FrameworkElement)e.NewValue).RenderTransform = new TranslateTransform(0, 0);

            if (e.OldValue != null)
            {
                ((FrameworkElement)e.OldValue).SizeChanged -= marquee.TargetChildOnSizeChanged;
            }

            if (e.NewValue != null)
            {
                ((FrameworkElement)e.NewValue).SizeChanged += marquee.TargetChildOnSizeChanged;
            }
        }

        private static void DuplicateChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FrameworkElement)e.NewValue).RenderTransform = new TranslateTransform(0, 0);
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
            ((MarqueeContainer)d).UpdateAnimation();
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
            if (TargetChild == null || DuplicateChild == null)
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
                StopTargetChildAnimation();
                StopDuplicateChildAnimation();
                return;
            }

            StartTargetChildAnimation();
            StartDuplicateChildAnimation();
            AssociatedObject.InvalidateVisual();
        }

        private void StartTargetChildAnimation()
        {
            var animation = CreateAnimation();

            TargetChild.RenderTransform = new TranslateTransform(AssociatedObject.Width, 0);
            TargetChild.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void StartDuplicateChildAnimation()
        {
            var animation = CreateAnimation();
            animation.BeginTime = TimeSpan.FromMilliseconds(CalculateTimeToScroll().TotalMilliseconds / 2);

            DuplicateChild.Opacity = 1;
            DuplicateChild.RenderTransform = new TranslateTransform(AssociatedObject.Width, 0);
            DuplicateChild.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void StopTargetChildAnimation()
        {
            TargetChild.RenderTransform.BeginAnimation(TranslateTransform.XProperty, null);
            TargetChild.RenderTransform = new TranslateTransform();
        }

        private void StopDuplicateChildAnimation()
        {
            DuplicateChild.Opacity = 0;
            DuplicateChild.RenderTransform.BeginAnimation(TranslateTransform.XProperty, null);
        }

        private DoubleAnimation CreateAnimation()
        {
            return new DoubleAnimation(AssociatedObject.ActualWidth, -TargetChild.ActualWidth - ChildMargin, CalculateTimeToScroll())
            {
                FillBehavior = FillBehavior.Stop,
                RepeatBehavior = RepeatBehavior.Forever,
            };
        }

        private TimeSpan CalculateTimeToScroll()
        {
            // Calculate time to scroll based on the target time to scroll across the panel.
            if (ScrollDuration.TotalMilliseconds == 0)
            {
                return TimeSpan.Zero;
            }

            var velocity = AssociatedObject.ActualWidth / ScrollDuration.TotalMilliseconds;
            var childScrollDistance = TargetChild.ActualWidth + AssociatedObject.ActualWidth;
            var scrollTime = childScrollDistance / velocity;

            return TimeSpan.FromMilliseconds(scrollTime);
        }
    }
}
