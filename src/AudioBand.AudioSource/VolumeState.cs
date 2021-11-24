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
        /// Less than 33% volume.
        /// </summary>
        Low,

        /// <summary>
        /// Between 33% and 66% volume.
        /// </summary>
        Mid,

        /// <summary>
        /// More than 66% volume.
        /// </summary>
        High,
    }
}
