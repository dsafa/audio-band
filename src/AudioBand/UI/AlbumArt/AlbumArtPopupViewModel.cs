using System;
using System.Diagnostics;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for the album art popup.
    /// </summary>
    public class AlbumArtPopupViewModel : ViewModelBase
    {
        private readonly IAppSettings _appSettings;
        private readonly AlbumArtPopup _model = new AlbumArtPopup();
        private readonly AlbumArtPopup _backup = new AlbumArtPopup();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtPopupViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="messageBus">The message bus.</param>
        public AlbumArtPopupViewModel(IAppSettings appSettings, IMessageBus messageBus)
        {
            MapSelf(appSettings.CurrentProfile.AlbumArtPopup, _model);
            MapSelf(appSettings.CurrentProfile.AlbumArtPopup, _backup);

            _appSettings = appSettings;
            appSettings.ProfileChanged += AppSettingsOnProfileChanged;
            UseMessageBus(messageBus);
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        [TrackState]
        public bool IsVisible
        {
            get => _model.IsVisible;
            set => SetProperty(_model, nameof(_model.IsVisible), value);
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
        /// Gets or sets the x position.
        /// </summary>
        [TrackState]
        public double XPosition
        {
            get => _model.XPosition;
            set => SetProperty(_model, nameof(_model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        [TrackState]
        public double Margin
        {
            get => _model.Margin;
            set => SetProperty(_model, nameof(_model.Margin), value);
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
            MapSelf(_model, _appSettings.CurrentProfile.AlbumArtPopup);
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appSettings.CurrentProfile.AlbumArtPopup, _model);
            RaisePropertyChangedAll();
        }
    }
}
