using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;
using Svg;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the previous button.
    /// </summary>
    public class PreviousButtonVM : ViewModelBase<PreviousButton>
    {
        private readonly SvgDocument _defaultPreviousButtonSvg;
        private readonly IResourceLoader _resourceLoader;
        private Image _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButtonVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        public PreviousButtonVM(IAppSettings appsettings, IResourceLoader resourceLoader)
            : base(appsettings.PreviousButton)
        {
            _defaultPreviousButtonSvg = resourceLoader.LoadSVGFromResource(Properties.Resources.previous);
            _resourceLoader = resourceLoader;
            LoadImage();
        }

        public Image Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        [PropertyChangeBinding(nameof(PreviousButton.ImagePath))]
        public string ImagePath
        {
            get => Model.ImagePath;
            set
            {
                if (SetProperty(nameof(Model.ImagePath), value))
                {
                    Image = _resourceLoader.TryLoadImageFromPath(value, _defaultPreviousButtonSvg.ToBitmap());
                }
            }
        }

        [PropertyChangeBinding(nameof(PreviousButton.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(PreviousButton.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(PreviousButton.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(PreviousButton.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(PreviousButton.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets the location of the button.
        /// </summary>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the button.
        /// </summary>
        public Size Size => new Size(Width, Height);

        /// <inheritdoc/>
        protected override void OnReset()
        {
            base.OnReset();
            LoadImage();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadImage();
        }

        private void LoadImage()
        {
            Image = _resourceLoader.TryLoadImageFromPath(ImagePath, _defaultPreviousButtonSvg.ToBitmap());
        }
    }
}
