using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the next button.
    /// </summary>
    public class NextButtonViewModel : ButtonViewModelBase<NextButton>
    {
        private readonly IAppSettings _appSettings;
        private readonly IAudioSession _audioSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        public NextButtonViewModel(IAppSettings appSettings, IDialogService dialogService, IAudioSession audioSession)
            : base(appSettings.NextButton, dialogService)
        {
            _appSettings = appSettings;
            _audioSession = audioSession;
            _appSettings.ProfileChanged += AppsSettingsOnProfileChanged;
            NextTrackCommand = new AsyncRelayCommand<object>(NextTrackCommandOnExecute);
            Content = new ButtonContentViewModel(Model.Content, new NextButton().Content, dialogService);
            TrackContentViewModel(Content);
        }

        /// <summary>
        /// Gets the button content.
        /// </summary>
        public ButtonContentViewModel Content { get; }

        /// <summary>
        /// Gets the next track command.
        /// </summary>
        public IAsyncCommand NextTrackCommand { get; }

        private async Task NextTrackCommandOnExecute(object arg)
        {
            if (_audioSession.CurrentAudioSource == null)
            {
                return;
            }

            await _audioSession.CurrentAudioSource.NextTrackAsync();
        }

        private void AppsSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appSettings.NextButton);
        }
    }
}
