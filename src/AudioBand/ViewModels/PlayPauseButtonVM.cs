using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Models;
using Svg;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    internal class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private static readonly SvgDocument DefaultPlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private static readonly SvgDocument DefaultPauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private readonly Track _track;
        private Image _playImage;
        private Image _pauseImage;

        public PlayPauseButtonVM(PlayPauseButton model, Track track)
            : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
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
                    PlayImage = LoadImage(value, DefaultPlayButtonSvg.ToBitmap());
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
                    PauseImage = LoadImage(value, DefaultPauseButtonSvg.ToBitmap());
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

                // Need padding so the image can fit properly in a button.
                return image.Scale(Width - 1, Height - 1);
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
            PlayImage = LoadImage(PlayImagePath, DefaultPlayButtonSvg.ToBitmap());
            PauseImage = LoadImage(PauseImagePath, DefaultPauseButtonSvg.ToBitmap());
        }
    }
}
