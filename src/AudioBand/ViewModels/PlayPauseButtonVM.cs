using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.ComponentModel;
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

        public Image PlayImage
        {
            get => _playImage;
            set => SetProperty(ref _playImage, value, alsoNotify: nameof(Image));
        }

        public string PlayImagePath
        {
            get => Model.PlayButtonImagePath;
            set
            {
                if (SetModelProperty(nameof(Model.PlayButtonImagePath), value))
                {
                    PlayImage = LoadImage(value, DefaultPlayButtonSvg.ToBitmap());
                }
            }
        }

        public Image PauseImage
        {
            get => _pauseImage;
            set => SetProperty(ref _pauseImage, value, alsoNotify: nameof(Image));
        }

        public string PauseImagePath
        {
            get => Model.PauseButtonImagePath;
            set
            {
                if (SetModelProperty(nameof(Model.PauseButtonImagePath), value))
                {
                    PauseImage = LoadImage(value, DefaultPauseButtonSvg.ToBitmap());
                }
            }
        }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value, alsoNotify: new []{nameof(Image), nameof(Size)});
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value, alsoNotify: new[] { nameof(Image), nameof(Size) });
        }

        public int XPosition
        {
            get => Model.XPosition;
            set => SetModelProperty(nameof(Model.XPosition), value, alsoNotify: nameof(Location));
        }

        public int YPosition
        {
            get => Model.YPosition;
            set => SetModelProperty(nameof(Model.YPosition), value, alsoNotify: nameof(Location));
        }

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
            _track.PropertyChanged += TrackOnPropertyChanged;
        }

        private void TrackOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(_track.IsPlaying))
            {
                RaisePropertyChanged(nameof(Image));
            }
        }
    }
}
