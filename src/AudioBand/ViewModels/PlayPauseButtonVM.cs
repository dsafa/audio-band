using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;
using Svg;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private readonly SvgDocument _defaultPlayButtonSvg;
        private readonly SvgDocument _defaultPauseButtonSvg;
        private readonly Track _track;
        private readonly IResourceLoader _resourceLoader;
        private Image _playImage;
        private Image _pauseImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButtonVM"/> class.
        /// </summary>
        /// <param name="appsettings">App settings.</param>
        /// <param name="resourceLoader">Resource loader</param>
        /// <param name="track">The track.</param>
        public PlayPauseButtonVM(IAppSettings appsettings, IResourceLoader resourceLoader, Track track)
            : base(appsettings.PlayPauseButton)
        {
            _track = track;
            SetupModelBindings(_track);
            _defaultPlayButtonSvg = resourceLoader.LoadSVGFromResource(Properties.Resources.play);
            _defaultPauseButtonSvg = resourceLoader.LoadSVGFromResource(Properties.Resources.pause);
            _resourceLoader = resourceLoader;
            LoadImages();
        }

        [AlsoNotify(nameof(Image))]
        public Image PlayImage
        {
            get => _playImage;
            set => SetProperty(ref _playImage, value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonImagePath))]
        public string PlayImagePath
        {
            get => Model.PlayButtonImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonImagePath), value))
                {
                    PlayImage = _resourceLoader.TryLoadImageFromPath(value, _defaultPlayButtonSvg.ToBitmap());
                }
            }
        }

        [AlsoNotify(nameof(Image))]
        public Image PauseImage
        {
            get => _pauseImage;
            set => SetProperty(ref _pauseImage, value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonImagePath))]
        public string PauseImagePath
        {
            get => Model.PauseButtonImagePath;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonImagePath), value))
                {
                    PauseImage = _resourceLoader.TryLoadImageFromPath(value, _defaultPauseButtonSvg.ToBitmap());
                }
            }
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public Image Image
        {
            get
            {
                var image = _track.IsPlaying ? PauseImage : PlayImage;
                return image;
            }
        }

        /// <summary>
        /// Gets the location of the button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);

        /// <inheritdoc/>
        protected override void OnReset()
        {
            base.OnReset();
            LoadImages();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadImages();
        }

        private void LoadImages()
        {
            PlayImage = _resourceLoader.TryLoadImageFromPath(PlayImagePath, _defaultPlayButtonSvg.ToBitmap());
            PauseImage = _resourceLoader.TryLoadImageFromPath(PauseImagePath, _defaultPauseButtonSvg.ToBitmap());
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
