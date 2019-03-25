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
    /// View model for the next button.
    /// </summary>
    public class NextButtonVM : ViewModelBase<NextButton>
    {
        private readonly SvgDocument _defaultNextButtonSvg;
        private readonly IResourceLoader _resourceLoader;
        private Image _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextButtonVM"/> class.
        /// </summary>
        /// <param name="appsettings">The appsettings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        public NextButtonVM(IAppSettings appsettings, IResourceLoader resourceLoader)
            : base(appsettings.NextButton)
        {
            _resourceLoader = resourceLoader;
            _defaultNextButtonSvg = resourceLoader.LoadSVGFromResource(Properties.Resources.next);
            LoadImage();
        }

        public Image Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        [PropertyChangeBinding(nameof(NextButton.ImagePath))]
        public string ImagePath
        {
            get => Model.ImagePath;
            set
            {
                if (SetProperty(nameof(Model.ImagePath), value))
                {
                    Image = _resourceLoader.TryLoadImageFromPath(value, _defaultNextButtonSvg.ToBitmap());
                }
            }
        }

        [PropertyChangeBinding(nameof(NextButton.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(NextButton.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(NextButton.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(NextButton.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(NextButton.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets the location of the next button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the next button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
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
            Image = _resourceLoader.TryLoadImageFromPath(ImagePath, _defaultNextButtonSvg.ToBitmap());
        }
    }
}
