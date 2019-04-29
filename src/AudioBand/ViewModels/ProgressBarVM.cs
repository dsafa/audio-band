using System;
using System.Diagnostics;
using System.Drawing;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the progress bar.
    /// </summary>
    public class ProgressBarVM : ViewModelBase<ProgressBar>
    {
        private readonly IAppSettings _appsettings;
        private readonly Track _track;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="track">The track model.</param>
        public ProgressBarVM(IAppSettings appsettings, IDialogService dialogService, Track track)
            : base(appsettings.ProgressBar)
        {
            _appsettings = appsettings;
            _track = track;
            SetupModelBindings(_track);
            DialogService = dialogService;

            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.ForegroundColor))]
        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetProperty(nameof(Model.ForegroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.BackgroundColor))]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(nameof(Model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the hover color.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.HoverColor))]
        public Color HoverColor
        {
            get => Model.HoverColor;
            set => SetProperty(nameof(Model.HoverColor), value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bar is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets the track progress.
        /// </summary>
        [PropertyChangeBinding(nameof(Track.TrackProgress))]
        public TimeSpan TrackProgress => _track.TrackProgress;

        /// <summary>
        /// Gets the track length.
        /// </summary>
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
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; set; }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.ProgressBar);
        }
    }
}
