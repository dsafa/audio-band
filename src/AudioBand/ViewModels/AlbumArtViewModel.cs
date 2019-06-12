using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
    public class AlbumArtViewModel : LayoutViewModelBase<AlbumArt>
    {
        private readonly IAppSettings _appsettings;
        private readonly IAudioSession _audioSession;
        private ImageSource _albumArt;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        public AlbumArtViewModel(IAppSettings appsettings, IDialogService dialogService, IAudioSession audioSession)
            : base(appsettings.AlbumArt)
        {
            DialogService = dialogService;
            _appsettings = appsettings;
            _audioSession = audioSession;

            appsettings.ProfileChanged += AppsettingsOnProfileChanged;
            audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
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
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.AlbumArt);
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.AlbumArt))
            {
                return;
            }

            AlbumArtUpdated(_audioSession.AlbumArt);
        }

        private void AlbumArtUpdated(Image albumArt)
        {
            if (albumArt == null)
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

            AlbumArt = albumArt.ToImageSource();
        }
    }
}
