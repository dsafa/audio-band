using AudioBand.Models;
using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class CustomLabelVM : ViewModelBase<CustomLabel>
    {
        public string Name
        {
            get => Model.Name;
            set => SetModelProperty(nameof(Model.Name), value);
        }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public string FontFamily
        {
            get => Model.FontFamily;
            set => SetModelProperty(nameof(Model.FontFamily), value);
        }

        public float FontSize
        {
            get => Model.FontSize;
            set => SetModelProperty(nameof(Model.FontSize), value);
        }

        public Color Color
        {
            get => Model.Color;
            set => SetModelProperty(nameof(Model.Color), value);
        }

        public string FormatString
        {
            get => Model.FormatString;
            set => SetModelProperty(nameof(Model.FormatString), value);
        }

        public CustomLabel.TextAlignment TextAlignment
        {
            get => Model.Alignment;
            set => SetModelProperty(nameof(Model.Alignment), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value, alsoNotify: nameof(Size));
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value, alsoNotify: nameof(Size));
        }

        public int XPosition
        {
            get => Model.XPosition;
            set => SetModelProperty(nameof(Model.XPosition), value, alsoNotify: nameof(Location));
        }

        public int YPosition
        {
            get => Model.YPosition;
            set => SetModelProperty(nameof(Model.YPosition), value, alsoNotify: nameof(Location));
        }

        public int ScrollSpeed
        {
            get => Model.ScrollSpeed;
            set => SetModelProperty(nameof(Model.ScrollSpeed), value);
        }

        public Point Location => new Point(XPosition, YPosition);

        public Size Size => new Size(Width, Height);

        public CustomLabelVM(CustomLabel model) : base(model) {}
    }
}