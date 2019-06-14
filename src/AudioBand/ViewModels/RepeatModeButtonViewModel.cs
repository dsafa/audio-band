using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the repeat mode button.
    /// </summary>
    public class RepeatModeButtonViewModel : ButtonViewModelBase<RepeatModeButton>
    {
        private readonly IAppSettings _appSettings;
        private readonly IAudioSession _audioSession;
        private RepeatMode _repeatMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatModeButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public RepeatModeButtonViewModel(IAppSettings appSettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(appSettings.RepeatModeButton, dialogService, messageBus)
        {
            _appSettings = appSettings;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
            _appSettings.ProfileChanged += AppSettingsOnProfileChanged;

            var resetState = new RepeatModeButton();
            RepeatOffContent = new ButtonContentViewModel(Model.RepeatOffContent, resetState.RepeatOffContent, dialogService);
            RepeatContextContent = new ButtonContentViewModel(Model.RepeatContextContent, resetState.RepeatContextContent, dialogService);
            RepeatTrackContent = new ButtonContentViewModel(Model.RepeatTrackContent, resetState.RepeatTrackContent, dialogService);
            CycleRepeatModeCommand = new AsyncRelayCommand<object>(CycleRepeatModeCommandOnExecute);

            TrackContentViewModel(RepeatOffContent);
            TrackContentViewModel(RepeatContextContent);
            TrackContentViewModel(RepeatTrackContent);
        }

        /// <summary>
        /// Gets the button content when repeat is off.
        /// </summary>
        public ButtonContentViewModel RepeatOffContent { get; }

        /// <summary>
        /// Gets the button content when repeat is on.
        /// </summary>
        public ButtonContentViewModel RepeatContextContent { get; }

        /// <summary>
        /// Gets the button content when repeat is on for the track.
        /// </summary>
        public ButtonContentViewModel RepeatTrackContent { get; }

        /// <summary>
        /// Gets the command to change the repeat mode.
        /// </summary>
        public IAsyncCommand CycleRepeatModeCommand { get; }

        /// <summary>
        /// Gets the current repeat mode.
        /// </summary>
        public RepeatMode RepeatMode
        {
            get => _repeatMode;
            private set => SetProperty(ref _repeatMode, value);
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _appSettings.RepeatModeButton);
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.RepeatMode))
            {
                return;
            }

            OnRepeatModeChanged(_audioSession.RepeatMode);
        }

        private void OnRepeatModeChanged(RepeatMode e)
        {
            RepeatMode = e;
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appSettings.RepeatModeButton, Model);
            RaisePropertyChangedAll();
        }

        private async Task CycleRepeatModeCommandOnExecute(object obj)
        {
            if (_audioSession.CurrentAudioSource == null)
            {
                return;
            }

            var nextRepeatMode = RepeatMode;
            switch (RepeatMode)
            {
                case RepeatMode.Off:
                    nextRepeatMode = RepeatMode.RepeatContext;
                    break;
                case RepeatMode.RepeatContext:
                    nextRepeatMode = RepeatMode.RepeatTrack;
                    break;
                case RepeatMode.RepeatTrack:
                    nextRepeatMode = RepeatMode.Off;
                    break;
            }

            await _audioSession.CurrentAudioSource.SetRepeatModeAsync(nextRepeatMode);
        }
    }
}
