using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace AudioBand.UI
{
    /// <summary>
    /// Compensates for the extra space due to the large thumb by setting a negative margin.
    /// </summary>
    public class TrackOffsetFix : Behavior<Track>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double extraHeight = (e.NewSize.Height - AssociatedObject.IncreaseRepeatButton.ActualHeight) / 2;
            var margin = AssociatedObject.Margin;
            margin.Top = -extraHeight;
            AssociatedObject.Margin = margin;
        }
    }
}
