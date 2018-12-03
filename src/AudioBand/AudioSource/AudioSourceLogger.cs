using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Logger injected into <see cref="IAudioSource.Logger"/>.
    /// </summary>
    internal class AudioSourceLogger : IAudioSourceLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceLogger"/> class with a name.
        /// </summary>
        /// <param name="name">Name of the audio source.</param>
        public AudioSourceLogger(string name)
        {
            _logger = LogManager.GetLogger("AudioSource:" + name);
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        /// <inheritdoc/>
        public void Debug(object value)
        {
            _logger.Debug(value);
        }

        /// <inheritdoc/>
        public void Info(string message)
        {
            _logger.Info(message);
        }

        /// <inheritdoc/>
        public void Info(object value)
        {
            _logger.Info(value);
        }

        /// <inheritdoc/>
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        /// <inheritdoc/>
        public void Warn(object value)
        {
            _logger.Warn(value);
        }

        /// <inheritdoc/>
        public void Error(string message)
        {
            _logger.Error(message);
        }

        /// <inheritdoc/>
        public void Error(object value)
        {
            _logger.Error(value);
        }
    }
}
