namespace AudioBand.Messages
{
    /// <summary>
    /// Global message that indicates that editing is ending.
    /// </summary>
    public enum EditEndMessage
    {
        /// <summary>
        /// User cancelled all current edits.
        /// </summary>
        Cancelled,

        /// <summary>
        /// User accepted all current edits.
        /// </summary>
        Accepted,

        /// <summary>
        /// Fixes unsaved changes when opening audiosource settings for the first time.
        /// </summary>
        SaveFix,
    }
}
