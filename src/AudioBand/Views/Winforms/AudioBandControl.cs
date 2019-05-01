using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AudioBand.Extensions;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Base class for audioband controls. Handles dpi changes.
    /// </summary>
    public class AudioBandControl : UserControl
    {
        private const double LogicalDpi = 96.0;
        private Size _logicalSize;
        private Point _logicalLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandControl"/> class.
        /// </summary>
        public AudioBandControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Gets or sets the logical size of the control.
        /// </summary>
        /// <remarks>This is the device independent size.</remarks>
        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public Size LogicalSize
        {
            get => _logicalSize;
            set
            {
                _logicalSize = value;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets the logical position of the control.
        /// </summary>
        /// <remarks>This is the device independent position.</remarks>
        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public Point LogicalLocation
        {
            get => _logicalLocation;
            set
            {
                _logicalLocation = value;
                UpdateLocation();
            }
        }

        /// <summary>
        /// Gets the dpi.
        /// </summary>
        public double Dpi { get; private set; } = LogicalDpi;

        /// <summary>
        /// Gets the scaling factor.
        /// </summary>
        public double ScalingFactor => Dpi / LogicalDpi;

        /// <summary>
        /// Gets the size scaled by the <see cref="ScalingFactor"/>.
        /// </summary>
        /// <param name="size">The size to scale.</param>
        /// <returns>The new scaled size.</returns>
        protected Size GetScaledSize(Size size)
        {
            return new Size((int)Math.Round(size.Width * ScalingFactor), (int)Math.Round(size.Height * ScalingFactor));
        }

        /// <summary>
        /// Gets the point scaled by the <see cref="ScalingFactor"/>.
        /// </summary>
        /// <param name="point">The point to scale.</param>
        /// <returns>The new scaled point.</returns>
        protected Point GetScaledPoint(Point point)
        {
            return new Point((int)Math.Round(point.X * ScalingFactor), (int)Math.Round(point.Y * ScalingFactor));
        }

        /// <summary>
        /// Cross thread safe refresh.
        /// </summary>
        protected void InvokeRefresh()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Refresh));
            }
            else
            {
                Refresh();
            }
        }

        /// <inheritdoc />
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateDpi(this.GetDpi());
        }

        /// <summary>
        /// Occurs when dpi changed after layout and resize.
        /// </summary>
        protected virtual void OnDpiChanged()
        {
        }

        /// <summary>
        /// Occurs when dpi is changing before layout and resize.
        /// </summary>
        protected virtual void OnDpiChanging()
        {
        }

        /// <summary>
        /// Updates the dpi of the control, then recursively calls it on the controls children.
        /// </summary>
        /// <param name="newDpi">The new dpi.</param>
        protected void UpdateDpi(double newDpi)
        {
            if (Math.Abs(newDpi - Dpi) < 0.0001)
            {
                return;
            }

            Dpi = newDpi;
            OnDpiChanging();
            UpdateLocation();
            UpdateSize();
            OnDpiChanged();

            foreach (AudioBandControl control in Controls.OfType<AudioBandControl>())
            {
                control.UpdateDpi(newDpi);
            }
        }

        private void UpdateSize()
        {
            var size = GetScaledSize(LogicalSize);
            Size = size;
            MinimumSize = size;
            MaximumSize = size;
        }

        private void UpdateLocation()
        {
            Location = GetScaledPoint(LogicalLocation);
        }
    }
}
