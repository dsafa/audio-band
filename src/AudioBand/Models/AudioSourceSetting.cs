namespace AudioBand.Models
{
    /// <summary>
    /// A key / value pair that represents a single setting for an audio source
    /// </summary>
    internal class AudioSourceSetting
    {
        /// <summary>
        /// Name of the setting
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the setting serialized as a string
        /// </summary>
        public string Value { get; set; }

        public AudioSourceSetting(){}

        public AudioSourceSetting(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}