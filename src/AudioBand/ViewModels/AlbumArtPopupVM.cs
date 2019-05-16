using System;
using System.Diagnostics;
using AudioBand.Models;
using AudioBand.Settings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art popup.
    /// </summary>
    public class AlbumArtPopupVM : ViewModelBase<AlbumArtPopup>
    {
        private readonly IAppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtPopupVM"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public AlbumArtPopupVM(IAppSettings appSettings)
            : base(appSettings.AlbumArtPopup)
        {
            _appSettings = appSettings;
            appSettings.ProfileChanged += AppSettingsOnProfileChanged;
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
