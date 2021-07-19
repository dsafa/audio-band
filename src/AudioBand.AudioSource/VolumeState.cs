namespace AudioBand.AudioSource
{
    /// <summary>
    /// Specifies the Volume State of the audio source.
    /// </summary>
    public enum VolumeState
    {
        /// <summary>
        /// No volume.
        /// </summary>
        Off,

        /// <summary>
        /// Less than 50% volume.
        /// </summary>
        Low,

        /// <summary>
        /// More than 50% volume.
        /// </summary>
        High,
    }
}
