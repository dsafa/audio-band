using System;
using System.Diagnostics;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the progress bar.
    /// </summary>
    public class ProgressBarViewModel : LayoutViewModelBase<ProgressBar>
    {
        private readonly IAppSettings _appsettings;
        private IAudioSource _audioSource;
        private TimeSpan _trackProgress;
        private TimeSpan _trackLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ProgressBarViewModel(IAppSettings appsettings, IDialogService dialogService)
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
