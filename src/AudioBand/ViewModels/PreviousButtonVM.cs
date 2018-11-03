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

        public string ImagePath
        {
            get => Model.ImagePath;
            set
            {
                if (SetModelProperty(nameof(Model.ImagePath), value))
                {
                    Image = LoadImage(value, DefaultPreviousButtonSvg.ToBitmap());
                }
            }
        }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value, alsoNotify: nameof(Image));
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value, alsoNotify: nameof(Image));
        }

        public int XPosition
        {
            get => XPosition;
            set => SetModelProperty(nameof(Model.XPosition), value, alsoNotify: nameof(Location));
        }

        public int YPosition
        {
            get => Model.YPosition;
            set => SetModelProperty(nameof(Model.YPosition), value, alsoNotify: nameof(Location));
        }

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public PreviousButtonVM(PreviousButton model) : base(model) {}
    }
}
