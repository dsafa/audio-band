using System.Windows;
using System.Windows.Controls.Primitives;

namespace AudioBand.UI
{
    /// <summary>
    /// Custom track that overlaps the repeat buttons.
    /// </summary>
    public class CustomTrack : Track
    {
        private double _distanceUnits = 1;

        /// <inheritdoc />
        public override double ValueFromPoint(Point pt)
        {
            if (ActualWidth == 0)
            {
                return 0;
            }

            return (pt.X / ActualWidth) * Maximum;
        }

        /// <inheritdoc />
        public override double ValueFromDistance(double horizontal, double vertical)
        {
            return horizontal * _distanceUnits;
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var max = Maximum;
            if (max == 0)
            {
                max = 1;
            }

            var progressX = Value / max * arrangeSize.Width;
            IncreaseRepeatButton.Arrange(new Rect(0, 0, arrangeSize.Width, Thumb.DesiredSize.Height));
            DecreaseRepeatButton.Arrange(new Rect(0, 0, progressX, Thumb.DesiredSize.Height));

            var thumbXPos = progressX - (Thumb.DesiredSize.Width / 2);
            Thumb.Arrange(new Rect(thumbXPos, 0, Thumb.DesiredSize.Width, Thumb.DesiredSize.Height));

            if (Maximum != 0)
            {
                _distanceUnits = Maximum / arrangeSize.Width;
            }

            return arrangeSize;
        }
    }
}
