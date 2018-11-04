using AudioBand.Extensions;
using AudioBand.Models;
using Svg;
using System.Drawing;
using System.IO;

namespace AudioBand.ViewModels
{
    internal class NextButtonVM : ViewModelBase<NextButton>
    {
        private static readonly SvgDocument DefaultNextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
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
                    Image = LoadImage(value, DefaultNextButtonSvg.ToBitmap());
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
            set => SetModelProperty(nameof(Model.Width), value, alsoNotify: new[] { nameof(Image), nameof(Size) });
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value, alsoNotify: new[] { nameof(Image), nameof(Size) });
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

        public Size Size => new Size(Width, Height);

        public NextButtonVM(NextButton model) : base(model) {}
    }
}
