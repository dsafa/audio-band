using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.Drawing;
using System.IO;

namespace AudioBand.ViewModels
{
    internal class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private static readonly SvgDocument DefaultPlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private static readonly SvgDocument DefaultPauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private readonly Track _track;
        private Image _playImage;
        private Image _pauseImage;

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
                return image.Scale(Width - 1, Height - 1); // To fit image in button
            }
        }

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public Size Size => new Size(Width, Height);

        public PlayPauseButtonVM(PlayPauseButton model, Track track) : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
            LoadImages();
        }

        private void LoadImages()
        {
            PlayImage = LoadImage(PlayImagePath, DefaultPlayButtonSvg.ToBitmap());
            PauseImage = LoadImage(PauseImagePath, DefaultPauseButtonSvg.ToBitmap());
        }

        protected override void OnReset()
        {
            base.OnReset();
            LoadImages();
        }

        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadImages();
        }
    }
}
