using System;
using System.Diagnostics;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art popup.
    /// </summary>
    public class AlbumArtPopupViewModel : ViewModelBase<AlbumArtPopup>
    {
        private readonly IAppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtPopupViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public AlbumArtPopupViewModel(IAppSettings appSettings)
            : base(appSettings.AlbumArtPopup)
        {
            _appSettings = appSettings;
            appSettings.ProfileChanged += AppSettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(AlbumArtPopup.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [PropertyChangeBinding(nameof(AlbumArtPopup.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(AlbumArtPopup.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(AlbumArtPopup.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        [PropertyChangeBinding(nameof(AlbumArtPopup.Margin))]
        public double Margin
        {
            get => Model.Margin;
            set => SetProperty(nameof(Model.Margin), value);
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appSettings.AlbumArtPopup);
        }
    }
}
