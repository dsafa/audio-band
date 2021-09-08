using AudioBand.Messages;
using AudioBand.Settings;
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for the general application.
    /// </summary>
    public class GeneralSettingsViewModel : ViewModelBase
    {
        private readonly IAppSettings _appsettings;
        private readonly Models.GeneralSettings _model = new Models.GeneralSettings();
        private readonly Models.GeneralSettings _backup = new Models.GeneralSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralSettingsViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="messageBus">The message bus.</param>
        public GeneralSettingsViewModel(IAppSettings appsettings, IDialogService dialogService, IMessageBus messageBus)
        {
            MapSelf(appsettings.CurrentProfile.GeneralSettings, _model);

            DialogService = dialogService;
            _appsettings = appsettings;
            appsettings.ProfileChanged += AppsettingsOnProfileChanged;
            UseMessageBus(messageBus);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [TrackState]
        public double Width
        {
            get => _model.Width;
            set => SetProperty(_model, nameof(_model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [TrackState]
        public double Height
        {
            get => _model.Height;
            set => SetProperty(_model, nameof(_model.Height), value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [TrackState]
        public Color BackgroundColor
        {
            get => _model.BackgroundColor;
            set => SetProperty(_model, nameof(_model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

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
            MapSelf(_model, _appsettings.CurrentProfile.GeneralSettings);
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appsettings.CurrentProfile.GeneralSettings, _model);
            RaisePropertyChangedAll();
        }
    }
}
