using System;
using System.Diagnostics;
using System.Drawing;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private readonly IAppSettings _appsettings;
        private readonly Track _track;
        private readonly IResourceLoader _resourceLoader;
        private IImage _playImage;
        private IImage _pauseImage;
        private IImage _playHoveredImage;
        private IImage _playClickedImage;
        private IImage _pauseHoveredImage;
        private IImage _pauseClickedImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButtonVM"/> class.
        /// </summary>
        /// <param name="appsettings">App settings.</param>
        /// <param name="resourceLoader">Resource loader</param>
        /// <param name="dialogService">The dialog service</param>
        /// <param name="track">The track.</param>
        public PlayPauseButtonVM(IAppSettings appsettings, IResourceLoader resourceLoader, IDialogService dialogService, Track track)
            : base(appsettings.PlayPauseButton)
        {
            DialogService = dialogService;
            _appsettings = appsettings;
            _track = track;
            SetupModelBindings(_track);
            _resourceLoader = resourceLoader;
            LoadImages();

            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        [AlsoNotify(nameof(Image))]
        public IImage PlayImage
        {
            get => _playImage;
            set => SetProperty(ref _playImage, value, trackChanges: false);
        }

        [AlsoNotify(nameof(Image))]
        public IImage PlayHoveredImage
        {
            get => _playHoveredImage;
            set => SetProperty(ref _playHoveredImage, value, trackChanges: false);
        }

        [AlsoNotify(nameof(Image))]
        public IImage PlayClickedImage
        {
            get => _playClickedImage;
            set => SetProperty(ref _playClickedImage, value, trackChanges: false);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonImagePath))]
        public string PlayImagePath
        {
            get => Model.PlayButtonImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonImagePath), value))
                {
                    PlayImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonHoveredImagePath))]
        public string PlayHoveredImagePath
        {
            get => Model.PlayButtonHoveredImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonHoveredImagePath), value))
                {
                    PlayHoveredImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonClickedImagePath))]
        public string PlayClickedImagePath
        {
            get => Model.PlayButtonClickedImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonClickedImagePath), value))
                {
                    PlayClickedImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        [AlsoNotify(nameof(Image))]
        public IImage PauseImage
        {
            get => _pauseImage;
            set => SetProperty(ref _pauseImage, value, trackChanges: false);
        }

        [AlsoNotify(nameof(Image))]
        public IImage PauseHoveredImage
        {
            get => _pauseHoveredImage;
            set => SetProperty(ref _pauseHoveredImage, value, trackChanges: false);
        }

        [AlsoNotify(nameof(Image))]
        public IImage PauseClickedImage
        {
            get => _pauseClickedImage;
            set => SetProperty(ref _pauseClickedImage, value, trackChanges: false);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonImagePath))]
        public string PauseImagePath
        {
            get => Model.PauseButtonImagePath;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonImagePath), value))
                {
                    PauseImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPauseImage);
                }
            }
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonHoveredImagePath))]
        public string PauseHoveredImagePath
        {
            get => Model.PauseButtonHoveredImagePath;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonHoveredImagePath), value))
                {
                    PauseHoveredImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPauseImage);
                }
            }
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonClickedImagePath))]
        public string PauseClickedImagePath
        {
            get => Model.PauseButtonClickedImagePath;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonClickedImagePath), value))
                {
                    PauseClickedImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPauseImage);
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
        public IImage Image => _track.IsPlaying ? PauseImage : PlayImage;

        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public IImage HoveredImage => _track.IsPlaying ? PauseHoveredImage : PlayHoveredImage;

        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public IImage ClickedImage => _track.IsPlaying ? PauseClickedImage : PlayClickedImage;

        [PropertyChangeBinding(nameof(PlayPauseButton.DefaultBackgroundColor))]
        public Color DefaultBackgroundColor
        {
            get => Model.DefaultBackgroundColor;
            set => SetProperty(nameof(Model.DefaultBackgroundColor), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.HoveredBackgroundColor))]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(nameof(Model.HoveredBackgroundColor), value);
        }

        [PropertyChangeBinding(nameof(PlayPauseButton.ClickedBackgroundColor))]
        public Color ClickedBackgroundColor
        {
            get => Model.ClickedBackgroundColor;
            set => SetProperty(nameof(Model.ClickedBackgroundColor), value);
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

        /// <summary>
        /// Gets the dialog service
        /// </summary>
        public IDialogService DialogService { get; }

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
            PlayImage = _resourceLoader.TryLoadImageFromPath(PlayImagePath, _resourceLoader.DefaultPlayImage);
            PlayHoveredImage = PlayImage;
            PlayClickedImage = PlayImage;
            PauseImage = _resourceLoader.TryLoadImageFromPath(PauseImagePath, _resourceLoader.DefaultPauseImage);
            PauseHoveredImage = PauseImage;
            PauseClickedImage = PauseImage;
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.PlayPauseButton);
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
