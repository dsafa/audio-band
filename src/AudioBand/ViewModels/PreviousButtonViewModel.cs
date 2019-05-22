using System.Threading.Tasks;
using System.Windows.Input;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the previous button.
    /// </summary>
    public class PreviousButtonViewModel : PlaybackButtonViewModelBase<PreviousButton>
    {
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public PreviousButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings, dialogService, appSettings.PreviousButton)
        {
            PreviousTrackCommand = new AsyncRelayCommand<object>(PreviousTrackCommandOnExecute);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the previous track command.
        /// </summary>
        public ICommand PreviousTrackCommand { get; }

        /// <inheritdoc />
        protected override PreviousButton GetReplacementModel()
        {
            return AppSettings.PreviousButton;
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        private async Task PreviousTrackCommandOnExecute(object arg)
        {
            if (_audioSource == null)
            {
                return;
            }

            await _audioSource.NextTrackAsync();
        }
    }
}
