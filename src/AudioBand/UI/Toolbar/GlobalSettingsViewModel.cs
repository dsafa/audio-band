using System;
using System.Diagnostics;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for Global AudioBand Settings.
    /// </summary>
    public class GlobalSettingsViewModel : ViewModelBase
    {
        private IAppSettings _appSettings;
        private readonly AudioBandSettings _model = new AudioBandSettings();
        private readonly AudioBandSettings _backup = new AudioBandSettings();

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
            set => SetProperty(_model, nameof(_model.HideIdleProfileInQuickMenu), value);
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

        /// <inheritdoc />
        protected override void OnReset()
        {
            base.OnReset();
            ResetObject(_model);
        }

        /// <inheritdoc />
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            MapSelf(_model, _backup);
        }

        /// <inheritdoc />
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            MapSelf(_backup, _model);
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(_model, _appSettings.AudioBandSettings);
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appSettings.AudioBandSettings, _model);
            RaisePropertyChangedAll();
        }
    }
}
