#pragma warning disable
using System.Collections.Generic;

namespace AudioBand.Settings.Models.V2
{
    public class AudioSourceSettings
    {
        public string AudioSourceName { get; set; }

        public List<AudioSourceSetting> Settings { get; set; }
    }
}
