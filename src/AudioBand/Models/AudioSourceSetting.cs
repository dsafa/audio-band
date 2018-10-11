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
        /// Value of the setting
        /// </summary>
        public object Value { get; set; }
    }
}