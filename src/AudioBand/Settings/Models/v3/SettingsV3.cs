using System.Collections.Generic;
using AudioBand.Models;

#pragma warning disable SA1300
#pragma warning disable 1591
#pragma warning disable SA1600
#pragma warning disable SA1402
namespace AudioBand.Settings.Models.v3
{
    public class SettingsV3
    {
        public const string DefaultProfileName = "Default Profile";

        public string Version { get; set; } = "3";

        public string AudioSource { get; set; }

        public string CurrentProfileName { get; set; }

        public Dictionary<string, ProfileV3> Profiles { get; set; }

        public List<AudioSourceSettings> AudioSourceSettings { get; set; }
    }

    public class ProfileV3
    {
        public AudioBand.Models.AudioBand AudioBandSettings { get; set; }

        public PreviousButton PreviousButtonSettings { get; set; }

        public PlayPauseButton PlayPauseButtonSettings { get; set; }

        public NextButton NextButtonSettings { get; set; }

        public ProgressBar ProgressBarSettings { get; set; }

        public AlbumArt AlbumArtSettings { get; set; }

        public AlbumArtPopup AlbumArtPopupSettings { get; set; }

        public List<CustomLabel> CustomLabelSettings { get; set; }
    }
}
