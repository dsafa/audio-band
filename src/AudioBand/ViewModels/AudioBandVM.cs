using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class AudioBandVM : ViewModelBase<Models.AudioBand>
    {
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

        public Size Size => new Size(Width, Height);

        public AudioBandVM(Models.AudioBand model) : base(model) {}
    }
}
