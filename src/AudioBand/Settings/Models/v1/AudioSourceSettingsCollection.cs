using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBand.Settings.Models.v1
{
    internal class AudioSourceSettingsCollection
    {
        public string AudioSourceName { get; set; }
        public List<AudioSourceSetting> Settings { get; set; } = new List<AudioSourceSetting>();
    }
}
