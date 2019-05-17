namespace AudioBand.Messages
{
    /// <summary>
    /// Message for dpi changes.
    /// </summary>
    public struct DpiChangedMessage
    {
        /// <summary>
        /// The new dpi.
        /// </summary>
        public readonly double NewDpi;

        /// <summary>
        /// Initializes a new instance of the <see cref="DpiChangedMessage"/> struct.
        /// </summary>
        /// <param name="newDpi">The new dpi.</param>
        public DpiChangedMessage(double newDpi)
        {
            NewDpi = newDpi;
        }
    }
}
