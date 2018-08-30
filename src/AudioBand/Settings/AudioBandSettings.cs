using AudioBand.ViewModels;

namespace AudioBand.Settings
{
    internal class AudioBandSettings
    {
        public string AudioSource { get; set; }
        public AppearanceViewModel AppearanceViewModel { get; set; } = new AppearanceViewModel();
    }
}
