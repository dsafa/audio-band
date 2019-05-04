using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private static readonly List<ButtonContentType> ContentTypes = Enum.GetValues(typeof(ButtonContentType)).Cast<ButtonContentType>().ToList();
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
        /// <param name="resourceLoader">Resource loader.</param>
        /// <param name="dialogService">The dialog service.</param>
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

        /// <summary>
        /// Gets or sets the play image.
        /// </summary>
        [AlsoNotify(nameof(Image))]
        public IImage PlayImage
        {
            get => _playImage;
            set => SetProperty(ref _playImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the play image when hovered.
        /// </summary>
        [AlsoNotify(nameof(HoveredImage))]
        public IImage PlayHoveredImage
        {
            get => _playHoveredImage;
            set => SetProperty(ref _playHoveredImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the play image when clicked.
        /// </summary>
        [AlsoNotify(nameof(ClickedImage))]
        public IImage PlayClickedImage
        {
            get => _playClickedImage;
            set => SetProperty(ref _playClickedImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the play image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonImagePath))]
        public string PlayImagePath
        {
            get => Model.PlayButtonImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonImagePath), value) && PlayButtonContentType == ButtonContentType.Image)
                {
                    PlayImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        /// <summary>
        /// Gets or sets the path of the play image when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonHoveredImagePath))]
        public string PlayHoveredImagePath
        {
            get => Model.PlayButtonHoveredImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonHoveredImagePath), value) && PlayButtonContentType == ButtonContentType.Image)
                {
                    PlayHoveredImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        /// <summary>
        /// Gets or sets the path of the play image when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonClickedImagePath))]
        public string PlayClickedImagePath
        {
            get => Model.PlayButtonClickedImagePath;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonClickedImagePath), value) && PlayButtonContentType == ButtonContentType.Image)
                {
                    PlayClickedImage = _resourceLoader.TryLoadImageFromPath(value, _resourceLoader.DefaultPlayImage);
                }
            }
        }

        /// <summary>
        /// Gets or sets the pause image.
        /// </summary>
        [AlsoNotify(nameof(Image))]
        public IImage PauseImage
        {
            get => _pauseImage;
            set => SetProperty(ref _pauseImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the pause image when hovered.
        /// </summary>
        [AlsoNotify(nameof(HoveredImage))]
        public IImage PauseHoveredImage
        {
            get => _pauseHoveredImage;
            set => SetProperty(ref _pauseHoveredImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the pause image when clicked.
        /// </summary>
        [AlsoNotify(nameof(ClickedImage))]
        public IImage PauseClickedImage
        {
            get => _pauseClickedImage;
            set => SetProperty(ref _pauseClickedImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the path of the pause image.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the path of the pause image when hovered.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the path of the pause image when clicked.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the play pause button is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets the current image.
        /// </summary>
        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public IImage Image => _track.IsPlaying ? PauseImage : PlayImage;

        /// <summary>
        /// Gets the current hovered image.
        /// </summary>
        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public IImage HoveredImage => _track.IsPlaying ? PauseHoveredImage : PlayHoveredImage;

        /// <summary>
        /// Gets the current clicked image.
        /// </summary>
        [PropertyChangeBinding(nameof(Track.IsPlaying))]
        public IImage ClickedImage => _track.IsPlaying ? PauseClickedImage : PlayClickedImage;

        /// <summary>
        /// Gets or sets the default background color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.DefaultBackgroundColor))]
        public Color DefaultBackgroundColor
        {
            get => Model.DefaultBackgroundColor;
            set => SetProperty(nameof(Model.DefaultBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.HoveredBackgroundColor))]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(nameof(Model.HoveredBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
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
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonContentType))]
        public ButtonContentType PlayButtonContentType
        {
            get => Model.PlayButtonContentType;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonContentType), value))
                {
                    UpdateCurrentPlayImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonContentType))]
        public ButtonContentType PauseButtonContentType
        {
            get => Model.PauseButtonContentType;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonContentType), value))
                {
                    UpdateCurrentPauseImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextFontFamily))]
        public string PlayButtonTextFontFamily
        {
            get => Model.PlayButtonTextFontFamily;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextFontFamily), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    UpdateCurrentPlayImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextFontFamily))]
        public string PauseButtonTextFontFamily
        {
            get => Model.PauseButtonTextFontFamily;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonTextFontFamily), value) && PauseButtonContentType == ButtonContentType.Text)
                {
                    UpdateCurrentPauseImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonText))]
        public string PlayButtonText
        {
            get => Model.PlayButtonText;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonText), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    UpdateCurrentPlayImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonText))]
        public string PauseButtonText
        {
            get => Model.PauseButtonText;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonText), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    UpdateCurrentPauseImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextHoverColor))]
        public Color PlayButtonTextColor
        {
            get => Model.PlayButtonTextColor;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PlayImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the play button when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextHoverColor))]
        public Color PlayButtonTextHoverColor
        {
            get => Model.PlayButtonTextHoverColor;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextClickedColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PlayHoveredImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the play button when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextClickedColor))]
        public Color PlayButtonTextClickedColor
        {
            get => Model.PlayButtonTextClickedColor;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextClickedColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PlayClickedImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextColor))]
        public Color PauseButtonTextColor
        {
            get => Model.PauseButtonTextColor;
            set
            {
                if (SetProperty(nameof(Model.PauseButtonTextColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PauseImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the pause button when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextHoverColor))]
        public Color PauseButtonTextHoverColor
        {
            get => Model.PauseButtonTextHoverColor;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextHoverColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PauseHoveredImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color for the pause button when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextClickedColor))]
        public Color PauseButtonTextClickedColor
        {
            get => Model.PauseButtonTextClickedColor;
            set
            {
                if (SetProperty(nameof(Model.PlayButtonTextClickedColor), value) && PlayButtonContentType == ButtonContentType.Text)
                {
                    PauseClickedImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, value);
                }
            }
        }

        /// <summary>
        /// Gets the button content types.
        /// </summary>
        public IEnumerable<ButtonContentType> ButtonContentTypes => ContentTypes;

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
            UpdateCurrentPlayImage();
            UpdateCurrentPauseImage();
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.PlayPauseButton);
        }

        private void UpdateCurrentPlayImage()
        {
            if (PlayButtonContentType == ButtonContentType.Image)
            {
                PlayImage = _resourceLoader.TryLoadImageFromPath(PlayImagePath, _resourceLoader.DefaultPlayImage);
                PlayHoveredImage = _resourceLoader.TryLoadImageFromPath(PlayHoveredImagePath, _resourceLoader.DefaultPlayImage);
                PlayClickedImage = _resourceLoader.TryLoadImageFromPath(PlayClickedImagePath, _resourceLoader.DefaultPlayImage);
            }
            else
            {
                PlayImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, PlayButtonTextColor);
                PlayHoveredImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, PlayButtonTextHoverColor);
                PlayClickedImage = new TextImage(PlayButtonText, PlayButtonTextFontFamily, PlayButtonTextClickedColor);
            }
        }

        private void UpdateCurrentPauseImage()
        {
            if (PauseButtonContentType == ButtonContentType.Image)
            {
                PauseImage = _resourceLoader.TryLoadImageFromPath(PauseImagePath, _resourceLoader.DefaultPauseImage);
                PauseHoveredImage = _resourceLoader.TryLoadImageFromPath(PauseHoveredImagePath, _resourceLoader.DefaultPauseImage);
                PauseClickedImage = _resourceLoader.TryLoadImageFromPath(PauseClickedImagePath, _resourceLoader.DefaultPauseImage);
            }
            else
            {
                PauseImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, PauseButtonTextColor);
                PauseHoveredImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, PauseButtonTextHoverColor);
                PauseClickedImage = new TextImage(PauseButtonText, PauseButtonTextFontFamily, PauseButtonTextClickedColor);
            }
        }
    }
}
