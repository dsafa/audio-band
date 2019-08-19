using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for the progress bar.
    /// </summary>
    public class ProgressBarViewModel : LayoutViewModelBase<ProgressBar>
    {
        private readonly IAppSettings _appsettings;
        private readonly IAudioSession _audioSession;
        private TimeSpan _trackProgress;
        private TimeSpan _trackLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public ProgressBarViewModel(IAppSettings appsettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(messageBus, appsettings.CurrentProfile.ProgressBar)
        {
            _appsettings = appsettings;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
            DialogService = dialogService;

            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [TrackState]
        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetProperty(Model, nameof(Model.ForegroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [TrackState]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(Model, nameof(Model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the hover color.
        /// </summary>
        [TrackState]
        public Color HoverColor
        {
            get => Model.HoverColor;
            set => SetProperty(Model, nameof(Model.HoverColor), value);
        }

        /// <summary>
        /// Gets or sets the track progress.
        /// </summary>
        public TimeSpan TrackProgress
        {
            get => _trackProgress;
            set
            {
                if (SetProperty(ref _trackProgress, value))
                {
                    _audioSession.CurrentAudioSource?.SetPlaybackProgressAsync(value);
                }
            }
        }

        /// <summary>
        /// Gets the track length.
        /// </summary>
        public TimeSpan TrackLength
        {
            get => _trackLength;
            private set => SetProperty(ref _trackLength, value);
        }

        /// <summary>
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; set; }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _appsettings.CurrentProfile.ProgressBar);
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appsettings.CurrentProfile.ProgressBar, Model);
            RaisePropertyChangedAll();
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IAudioSession.SongProgress):
                    OnProgressChanged(_audioSession.SongProgress);
                    break;
                case nameof(IAudioSession.SongLength):
                    OnSongLengthChanged(_audioSession.SongLength);
                    break;
            }
        }

        private void OnSongLengthChanged(TimeSpan length)
        {
            TrackLength = length;
        }

        private void OnProgressChanged(TimeSpan progress)
        {
            _trackProgress = progress;
            RaisePropertyChanged(nameof(TrackProgress));
        }
    }
}
