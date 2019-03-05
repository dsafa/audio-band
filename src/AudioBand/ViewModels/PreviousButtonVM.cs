using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Models;
using Svg;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the previous button.
    /// </summary>
    internal class PreviousButtonVM : ViewModelBase<PreviousButton>
    {
        private static readonly SvgDocument DefaultPreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private Image _image;

        public PreviousButtonVM(PreviousButton model)
            : base(model)
        {
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
                    Image = LoadImage(value, DefaultPreviousButtonSvg.ToBitmap());
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
            Image = LoadImage(ImagePath, DefaultPreviousButtonSvg.ToBitmap());
        }
    }
}
