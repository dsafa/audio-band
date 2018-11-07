using AudioBand.Extensions;
using AudioBand.Models;
using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class AlbumArtPopupVM : ViewModelBase<AlbumArtPopup>
    {
        private readonly Track _track;

        [PropertyChangeBinding(nameof(AlbumArtPopup.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.Width))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.Height))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.XPosition))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(AlbumArtPopup.Margin))]
        public int Margin
        {
            get => Model.Margin;
            set => SetProperty(nameof(Model.Margin), value);
        }

        [PropertyChangeBinding(nameof(Track.AlbumArt))]
        public Image AlbumArt => _track.AlbumArt?.Resize(Width, Height);

        public AlbumArtPopupVM(AlbumArtPopup model, Track track) : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
        }
    }
}
