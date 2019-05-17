using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Commands;
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
        private IAudioSource _audioSource;
        private TimeSpan _trackProgress;
        private TimeSpan _trackLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ProgressBarVM(IAppSettings appsettings, IDialogService dialogService)
            : base(appsettings.ProgressBar)
        {
            _appsettings = appsettings;
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
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.Height))]
        [AlsoNotify(nameof(Size))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(ProgressBar.YPosition))]
        public double YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets or sets the track progress.
        /// </summary>
        public TimeSpan TrackProgress
        {
            get => _trackProgress;
            set
            {
                if (SetProperty(ref _trackProgress, value, trackChanges: false))
                {
                    _audioSource?.SetPlaybackProgressAsync(value);
                }
            }
        }

        /// <summary>
        /// Gets the track length.
        /// </summary>
        public TimeSpan TrackLength
        {
            get => _trackLength;
            private set => SetProperty(ref _trackLength, value, false);
        }

        /// <summary>
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; set; }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.ProgressBar);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                _audioSource.TrackProgressChanged -= AudioSourceOnTrackProgressChanged;
                _audioSource.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
                TrackProgress = TimeSpan.Zero;
                TrackLength = TimeSpan.Zero;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                TrackLength = TimeSpan.Zero;
                TrackProgress = TimeSpan.Zero;
                return;
            }

            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            TrackLength = e.TrackLength;
        }

        private void AudioSourceOnTrackProgressChanged(object sender, TimeSpan e)
        {
            _trackProgress = e;
            RaisePropertyChanged(nameof(TrackProgress));
        }
    }
}
