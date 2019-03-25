using System;
using System.Drawing;
using AudioBand.Models;
using AudioBand.Settings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the progress bar.
    /// </summary>
    public class ProgressBarVM : ViewModelBase<ProgressBar>
    {
        private readonly Track _track;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings</param>
        /// <param name="dialogService">The dialog service</param>
        /// <param name="track">The track model.</param>
        public ProgressBarVM(IAppSettings appsettings, IDialogService dialogService, Track track)
            : base(appsettings.ProgressBar)
        {
            _track = track;
            SetupModelBindings(_track);
            DialogService = dialogService;
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

        /// <summary>
        /// Gets the dialog service
        /// </summary>
        public IDialogService DialogService { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
