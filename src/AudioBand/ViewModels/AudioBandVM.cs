namespace AudioBand.ViewModels
{
    internal class AudioBandVM : ViewModelBase<Models.AudioBand>
    {
        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value);
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value);
        }

        public AudioBandVM(Models.AudioBand model) : base(model) {}
    }
}
