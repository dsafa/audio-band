using System;
using System.Drawing;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the progress bar.
    /// </summary>
    internal class ProgressBarVM : ViewModelBase<ProgressBar>
    {
        private readonly Track _track;

        public ProgressBarVM(ProgressBar model, Track track)
            : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
        }

        [PropertyChangeBinding(nameof(ProgressBar.ForegroundColor))]
        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetProperty(nameof(Model.ForegroundColor), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.BackgroundColor))]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(nameof(Model.BackgroundColor), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(ProgressBar.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(Track.TrackProgress))]
        public TimeSpan TrackProgress => _track.TrackProgress;

        [PropertyChangeBinding(nameof(Track.TrackLength))]
        public TimeSpan TrackLength => _track.TrackLength;

        /// <summary>
        /// Gets the location of the progress bar.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the progress bar.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);
    }
}
