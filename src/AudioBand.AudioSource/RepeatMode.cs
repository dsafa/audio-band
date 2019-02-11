namespace AudioBand.AudioSource
{
    /// <summary>
    /// Specifies the repeat mode of the audio source.
    /// </summary>
    public enum RepeatMode
    {
        /// <summary>
        /// No repeat.
        /// </summary>
        Off,

        /// <summary>
        /// Repeat the current context.
        /// </summary>
        RepeatContext,

        /// <summary>
        /// Repeat the current track.
        /// </summary>
        RepeatTrack,
    }
}
