using AudioBand.Messages;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for Global AudioBand Settings.
    /// </summary>
    public class GlobalSettingsViewModel : ViewModelBase
    {
        private IAppSettings _appSettings;
        private readonly Models.AudioBandSettings _model = new Models.AudioBandSettings();
        private readonly Models.AudioBandSettings _backup = new Models.AudioBandSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingsViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="messageBus">The message bus.</param>
        public GlobalSettingsViewModel(IAppSettings appsettings, IMessageBus messageBus)
        {
            MapSelf(appsettings.AudioBandSettings, _model);

            _appSettings = appsettings;
            UseMessageBus(messageBus);
        }

        /// <summary>
        /// Gets or sets whether to use Automatic Idle Profile.
        /// </summary>
        [TrackState]
        public bool UseAutomaticIdleProfile
        {
            get => _model.UseAutomaticIdleProfile;
            set => SetProperty(_model, nameof(_model.UseAutomaticIdleProfile), value);
        }

        /// <summary>
        /// Gets or sets whether to hide the Idle Profile in the Quick Menu.
        /// </summary>
        [TrackState]
        public bool HideIdleProfileInQuickMenu
        {
            get => _model.HideIdleProfileInQuickMenu;
            set => SetProperty(_model, nameof(_model.UseAutomaticIdleProfile), value);
        }

        /// <summary>
        /// Gets or sets how to long to wait before AudioBand goes into an idle state.
        /// </summary>
        [TrackState]
        public int ShouldGoIdleAfterInSeconds
        {
            get => _model.ShouldGoIdleAfterInSeconds;
            set => SetProperty(_model, nameof(_model.ShouldGoIdleAfterInSeconds), value);
        }
    }
}
