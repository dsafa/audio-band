using System.Threading.Tasks;
using System.Windows.Input;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the next button.
    /// </summary>
    public class NextButtonViewModel : PlaybackButtonViewModelBase<NextButton>
    {
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public NextButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings, dialogService, appSettings.NextButton)
        {
            NextTrackCommand = new AsyncRelayCommand<object>(NextTrackCommandOnExecute);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the next track command.
        /// </summary>
        public ICommand NextTrackCommand { get; }

        /// <inheritdoc />
        protected override NextButton GetReplacementModel()
        {
            return AppSettings.NextButton;
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        private async Task NextTrackCommandOnExecute(object arg)
        {
            if (_audioSource == null)
            {
                return;
            }

            await _audioSource.NextTrackAsync();
        }
    }
}
