using System.Drawing;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;
using Svg;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the album art.
    /// </summary>
    public class AlbumArtVM : ViewModelBase<AlbumArt>
    {
        private readonly SvgDocument _defaultAlbumArtPlaceholderSvg;
        private readonly Track _track;
        private readonly IResourceLoader _resourceLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        /// <param name="track">The track model.</param>
        public AlbumArtVM(IAppSettings appsettings, IResourceLoader resourceLoader, Track track)
            : base(appsettings.AlbumArt)
        {
            _track = track;
            SetupModelBindings(_track);
            _defaultAlbumArtPlaceholderSvg = resourceLoader.LoadSVGFromResource(Properties.Resources.placeholder_album);
            _resourceLoader = resourceLoader;
            LoadAlbumArtPlaceholder();
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
                    LoadAlbumArtPlaceholder();
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
            LoadAlbumArtPlaceholder();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadAlbumArtPlaceholder();
        }

        private void LoadAlbumArtPlaceholder()
        {
            _track.UpdatePlaceholder(_resourceLoader.TryLoadImageFromPath(Model.PlaceholderPath, _defaultAlbumArtPlaceholderSvg.ToBitmap()));
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
