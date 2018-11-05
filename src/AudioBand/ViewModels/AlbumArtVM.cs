using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.Drawing;
using System.IO;

namespace AudioBand.ViewModels
{
    internal class AlbumArtVM : ViewModelBase<AlbumArt>
    {
        private readonly Track _track;
        private static readonly SvgDocument DefaultAlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));

        [PropertyChangeBinding(nameof(Models.AlbumArt.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(Models.AlbumArt.PlaceholderPath))]
        public string PlaceholderPath
        {
            get => Model.PlaceholderPath;
            set
            {
                if (SetProperty(nameof(Model.PlaceholderPath), value))
                {
                    LoadPlaceholder();
                }
            }
        }

        [PropertyChangeBinding(nameof(Track.AlbumArt))]
        public Image AlbumArt => (_track.AlbumArt ?? _track.PlaceholderImage).Resize(Width, Height);

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public Size Size => new Size(Width, Height);

        public AlbumArtVM(AlbumArt model, Track track) : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
        }

        private void LoadPlaceholder()
        {
            _track.PlaceholderImage = LoadImage(Model.PlaceholderPath, DefaultAlbumArtPlaceholderSvg.ToBitmap());
        }
    }
}
