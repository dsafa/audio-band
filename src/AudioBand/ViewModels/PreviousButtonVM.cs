using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.Drawing;
using System.IO;

namespace AudioBand.ViewModels
{
    internal class PreviousButtonVM : ViewModelBase<PreviousButton>
    {
        private static readonly SvgDocument DefaultPreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private Image _image;

        public Image Image
        {
            get => _image.Scale(Width - 1, Height - 1);
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

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public Size Size => new Size(Width, Height);

        public PreviousButtonVM(PreviousButton model) : base(model)
        {
            LoadImage();
        }

        private void LoadImage()
        {
            Image = LoadImage(ImagePath, DefaultPreviousButtonSvg.ToBitmap());
        }

        protected override void OnReset()
        {
            base.OnReset();
            LoadImage();
        }

        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            LoadImage();
        }
    }
}
