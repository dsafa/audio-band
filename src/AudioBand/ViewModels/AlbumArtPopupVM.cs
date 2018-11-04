using AudioBand.Extensions;
using AudioBand.Models;
using System.ComponentModel;
using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class AlbumArtPopupVM : ViewModelBase<AlbumArtPopup>
    {
        private readonly Track _track;

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value);
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value);
        }

        public int XPosition
        {
            get => Model.XPosition;
            set => SetModelProperty(nameof(Model.XPosition), value);
        }

        public int Margin
        {
            get => Model.Margin;
            set => SetModelProperty(nameof(Model.Margin), value);
        }

        public Image AlbumArt => (_track.AlbumArt ?? _track.PlaceholderImage).Resize(Width, Height);

        public AlbumArtPopupVM(AlbumArtPopup model, Track track) : base(model)
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
    }
}
