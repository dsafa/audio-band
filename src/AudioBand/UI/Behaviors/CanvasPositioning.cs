using AudioBand.Models;
using System.Windows;
using System.Windows.Controls;

namespace AudioBand.UI
{
    /// <summary>
    /// Attached properties for changing <see cref="Canvas"/> positioning based on <see cref="PositionAnchor"/> values.
    /// </summary>
    public static class CanvasPositioning
    {
        /// <summary>
        /// Identifies the <see cref="GetAnchor"/> attached property.
        /// </summary>
        public static readonly DependencyProperty AnchorProperty
            = DependencyProperty.RegisterAttached("Anchor", typeof(PositionAnchor), typeof(CanvasPositioning), new PropertyMetadata(PositionAnchor.TopLeft, AnchorPropertyChangedCallback));

        /// <summary>
        /// Identifies the <see cref="GetAnchorXDistance"/> attached property.
        /// </summary>
        public static readonly DependencyProperty AnchorXDistanceProperty
            = DependencyProperty.RegisterAttached("AnchorXDistance", typeof(double), typeof(CanvasPositioning), new PropertyMetadata(0.0, AnchorXPropertyChangedCallback));

        /// <summary>
        /// Identifies the <see cref="GetAnchorYDistance"/> attached property.
        /// </summary>
        public static readonly DependencyProperty AnchorYDistanceProperty
            = DependencyProperty.RegisterAttached("AnchorYDistance", typeof(double), typeof(CanvasPositioning), new PropertyMetadata(0.0, AnchorYPropertyChangedCallback));

        /// <summary>
        /// Gets the value of the <see cref="AnchorProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <returns>The anchor value.</returns>
        public static PositionAnchor GetAnchor(UIElement d)
        {
            return (PositionAnchor)d.GetValue(AnchorProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="AnchorProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <param name="anchor">The value to set.</param>
        public static void SetAnchor(UIElement d, PositionAnchor anchor)
        {
            d.SetValue(AnchorProperty, anchor);
        }

        /// <summary>
        /// Gets the value of the <see cref="AnchorXDistanceProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <returns>The anchor x distance.</returns>
        public static double GetAnchorXDistance(UIElement d)
        {
            return (double)d.GetValue(AnchorXDistanceProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="AnchorXDistanceProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAnchorXDistance(UIElement d, double value)
        {
            d.SetValue(AnchorXDistanceProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="AnchorYDistanceProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <returns>The anchor y distance.</returns>
        public static double GetAnchorYDistance(UIElement d)
        {
            return (double)d.GetValue(AnchorYDistanceProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="AnchorYDistanceProperty"/> property.
        /// </summary>
        /// <param name="d">The element.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAnchorYDistance(UIElement d, double value)
        {
            d.SetValue(AnchorYDistanceProperty, value);
        }

        private static void AnchorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(Canvas.LeftProperty, DependencyProperty.UnsetValue);
            d.SetValue(Canvas.RightProperty, DependencyProperty.UnsetValue);
            d.SetValue(Canvas.TopProperty, DependencyProperty.UnsetValue);
            d.SetValue(Canvas.BottomProperty, DependencyProperty.UnsetValue);

            UpdateX((UIElement)d);
            UpdateY((UIElement)d);
        }

        private static void AnchorXPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateX((UIElement)d);
        }

        private static void AnchorYPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateY((UIElement)d);
        }

        private static void UpdateX(UIElement element)
        {
            var anchor = GetAnchor(element);
            var x = GetAnchorXDistance(element);

            if (anchor.HasFlag(PositionAnchor.Left))
            {
                Canvas.SetLeft(element, x);
            }
            else if (anchor.HasFlag(PositionAnchor.Right))
            {
                Canvas.SetRight(element, x);
            }
        }

        private static void UpdateY(UIElement element)
        {
            var anchor = GetAnchor(element);
            var y = GetAnchorYDistance(element);

            if (anchor.HasFlag(PositionAnchor.Top))
            {
                Canvas.SetTop(element, y);
            }
            else if (anchor.HasFlag(PositionAnchor.Bottom))
            {
                Canvas.SetBottom(element, y);
            }
        }
    }
}
