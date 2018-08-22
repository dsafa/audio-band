using AudioBand.ViewModels;

namespace AudioBand.Settings
{
    internal class AudioBandSettings
    {
        public string AudioSource { get; set; }
        public AudioBandAppearance AudioBandAppearance { get; set; } = new AudioBandAppearance();
    }
}
