using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
/// <summary>
    /// View model for the volume button.
    /// </summary>
    public class VolumeButtonViewModel : ButtonViewModelBase<VolumeButton>
    {
        private readonly IAppSettings _appSettings;
        private readonly IAudioSession _audioSession;
        private int _volume;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">App settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public VolumeButtonViewModel(IAppSettings appSettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(appSettings.CurrentProfile.VolumeButton, dialogService, messageBus)
        {
            _appSettings = appSettings;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
            _appSettings.ProfileChanged += AppSettingsOnProfileChanged;
            ChangeVolumeCommand = new AsyncRelayCommand<object>(ChangeVolumeCommandOnExecute);

            var resetBase = new VolumeButton();
            NoVolumeContent = new ButtonContentViewModel(Model.NoVolumeContent, resetBase.NoVolumeContent, dialogService);
            LowVolumeContent = new ButtonContentViewModel(Model.LowVolumeContent, resetBase.LowVolumeContent, dialogService);
            HighVolumeContent = new ButtonContentViewModel(Model.HighVolumeContent, resetBase.HighVolumeContent, dialogService);
            TrackContentViewModel(NoVolumeContent);
            TrackContentViewModel(LowVolumeContent);
            TrackContentViewModel(HighVolumeContent);
        }

        /// <summary>
        /// Gets the view model for the button when there is no volume.
        /// </summary>
        public ButtonContentViewModel NoVolumeContent { get; }

        /// <summary>
        /// Gets the view model for the button with low volume.
        /// </summary>
        public ButtonContentViewModel LowVolumeContent { get; }

        /// <summary>
        /// Gets the view model for the button with high volume.
        /// </summary>
        public ButtonContentViewModel HighVolumeContent { get; }

        /// <summary>
        /// Gets the play pause command.
        /// </summary>
        public IAsyncCommand ChangeVolumeCommand { get; }

        /// <summary>
        /// Gets the current VolumeState.
        /// </summary>
        public VolumeState CurrentVolumeState
        {
            get
            {
                if (_volume == 0)
                {
                    return VolumeState.Off;
                }
                else if (_volume < 50)
                {
                    return VolumeState.Low;
                }
                else
                {
                    return VolumeState.High;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Volume.
        /// </summary>
        public int Volume
        {
            get => _volume;
            private set => SetProperty(ref _volume, value);
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _appSettings.CurrentProfile.VolumeButton);
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appSettings.CurrentProfile.VolumeButton, Model);
            RaisePropertyChangedAll();
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.Volume))
            {
                return;
            }

            OnVolumeChanged(_audioSession.Volume);
        }

        private void OnVolumeChanged(int newVolume)
        {
            Volume = newVolume;
        }

        private async Task ChangeVolumeCommandOnExecute(object arg)
        {
            if (_audioSession.CurrentAudioSource == null)
            {
                return;
            }

            await _audioSession.CurrentAudioSource.SetVolumeAsync(_volume);
        }
    }
}
