using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace AudioBand.ViewModels
{
    internal class AlbumArtVM : ViewModelBase<AlbumArt>
    {
        private readonly Track _track;
        private static readonly SvgDocument DefaultAlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value, alsoNotify: nameof(AlbumArt));
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value, alsoNotify: nameof(AlbumArt));
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

        public string PlaceholderPath
        {
            get => Model.PlaceholderPath;
            set
            {
                SetModelProperty(nameof(Model.PlaceholderPath), value);
                LoadPlaceholder();
            }
        }

        public Image AlbumArt => (_track.AlbumArt ?? _track.PlaceholderImage).Resize(Width, Height);

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public AlbumArtVM(AlbumArt model, Track track) : base(model)
        {
            _track = track;
            _track.PropertyChanged += TrackOnPropertyChanged;
        }

        private void TrackOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(_track.AlbumArt))
            {
                RaisePropertyChanged(nameof(AlbumArt));
            }
        }

        private void LoadPlaceholder()
        {
            _track.PlaceholderImage = LoadImage(Model.PlaceholderPath, DefaultAlbumArtPlaceholderSvg.ToBitmap());
        }
    }
}
