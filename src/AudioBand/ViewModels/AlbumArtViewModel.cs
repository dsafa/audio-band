using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art.
    /// </summary>
    public class AlbumArtViewModel : ViewModelBase<AlbumArt>
    {
        private readonly IAppSettings _appsettings;
        private IAudioSource _audioSource;
        private ImageSource _albumArt;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public AlbumArtViewModel(IAppSettings appsettings, IDialogService dialogService)
            : base(appsettings.AlbumArt)
        {
            DialogService = dialogService;
            _appsettings = appsettings;

            appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.YPosition))]
        public double YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets or sets the placeholder path.
        /// </summary>
        [PropertyChangeBinding(nameof(Models.AlbumArt.PlaceholderPath))]
        public string PlaceholderPath
        {
            get => Model.PlaceholderPath;
            set => SetProperty(nameof(Model.PlaceholderPath), value);
        }

        /// <summary>
        /// Gets the current album art image.
        /// </summary>
        public ImageSource AlbumArt
        {
            get => _albumArt;
            private set => SetProperty(ref _albumArt, value, false);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.AlbumArt);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                AlbumArt = null;
                _audioSource.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                AlbumArt = null;
                return;
            }

            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            if (e.AlbumArt == null)
            {
                try
                {
                    AlbumArt = new BitmapImage(new Uri(PlaceholderPath));
                }
                catch
                {
                    AlbumArt = null;
                }

                return;
            }

            AlbumArt = e.AlbumArt.ToImageSource();
        }
    }
}
