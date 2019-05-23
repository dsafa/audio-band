#pragma warning disable
using System.Collections.Generic;

namespace AudioBand.Settings.Models.V1
{
    internal class AudioSourceSettingsCollection
    {
        public string Name { get; set; }

        public List<AudioSourceSetting> Settings { get; set; } = new List<AudioSourceSetting>();
    }
}
