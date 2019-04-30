namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides access to logging facilities for a audio source.
    /// </summary>
    public interface IAudioSourceLogger
    {
        /// <summary>
        /// Log a debug level message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Debug(string message);

        /// <summary>
        /// Log a debug level message.
        /// </summary>
        /// <param name="value">Value to be logged.</param>
        void Debug(object value);

        /// <summary>
        /// Log an info level message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Info(string message);

        /// <summary>
        /// Log an info level message.
        /// </summary>
        /// <param name="value">Value to be logged.</param>
        void Info(object value);

        /// <summary>
        /// Log a warning level message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Warn(string message);

        /// <summary>
        /// Log a warning level message.
        /// </summary>
        /// <param name="value">Value to be logged.</param>
        void Warn(object value);

        /// <summary>
        /// Log an error level message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Error(string message);

        /// <summary>
        /// Log an error level message.
        /// </summary>
        /// <param name="value">Value to be logged.</param>
        void Error(object value);
    }
}
