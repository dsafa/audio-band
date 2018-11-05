using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class AudioBandVM : ViewModelBase<Models.AudioBand>
    {
        [PropertyChangeBinding(nameof(Models.AudioBand.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(Models.AudioBand.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        public Size Size => new Size(Width, Height);

        public AudioBandVM(Models.AudioBand model) : base(model) {}
    }
}
