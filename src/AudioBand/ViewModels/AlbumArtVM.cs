using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.Settings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art.
    /// </summary>
    public class AlbumArtVM : ViewModelBase<AlbumArt>
    {
        private readonly IAppSettings _appsettings;
        private IAudioSource _audioSource;
        private ImageSource _albumArt;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public AlbumArtVM(IAppSettings appsettings, IDialogService dialogService)
            : base(appsettings.AlbumArt)
        {
            DialogService = dialogService;
            _appsettings = appsettings;

            appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.YPosition))]
        public double YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.PlaceholderPath))]
        public string PlaceholderPath
        {
            get => Model.PlaceholderPath;
            set => SetProperty(nameof(Model.PlaceholderPath), value);
        }

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
