using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Models;
using Svg;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art.
    /// </summary>
    public class AlbumArtVM : ViewModelBase<AlbumArt>
    {
        private static readonly SvgDocument DefaultAlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));
        private readonly Track _track;

        public AlbumArtVM(AlbumArt model, Track track)
            : base(model)
        {
            _track = track;
            SetupModelBindings(_track);
            LoadPlaceholder();
        }

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
        public Image AlbumArt => _track.AlbumArt;

        /// <summary>
        /// Gets the location of the album art.
        /// </summary>
        /// <remarks>This property is exists so the designer can bind to it.</remarks>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the album art.
        /// </summary>
        /// <remarks>This property exists so that designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);

        /// <inheritdoc/>
        protected override void OnReset()
        {
            base.OnReset();
            LoadPlaceholder();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadPlaceholder();
        }

        private void LoadPlaceholder()
        {
            _track.UpdatePlaceholder(LoadImage(Model.PlaceholderPath, DefaultAlbumArtPlaceholderSvg.ToBitmap()));
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
