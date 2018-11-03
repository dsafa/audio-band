using System.Collections.Generic;

namespace AudioBand.Settings.Models.v2
{
    internal class AudioSourceSettingsCollection
    {
        public string AudioSourceName { get; set; }
        public List<AudioSourceSetting> Settings { get; set; }
    }
}
