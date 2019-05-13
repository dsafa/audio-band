using AudioBand.AudioSource;
using AudioBand.Logging;
using NLog;

namespace AudioSourceHost
{
    /// <summary>
    /// Logger passed to <see cref="IAudioSource.Logger"/>.
    /// </summary>
    public class AudioSourceLogger : IAudioSourceLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceLogger"/> class.
        /// </summary>
        /// <param name="audiosourceName">The name of the audio source.</param>
        public AudioSourceLogger(string audiosourceName)
        {
            _logger = AudioBandLogManager.GetLogger($"AudioSource({audiosourceName})");
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        /// <inheritdoc />
        public void Debug(object value)
        {
            _logger.Debug(value);
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            _logger.Error(message);
        }

        /// <inheritdoc />
        public void Error(object value)
        {
            _logger.Error(value);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            _logger.Info(message);
        }

        /// <inheritdoc />
        public void Info(object value)
        {
            _logger.Info(value);
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        /// <inheritdoc />
        public void Warn(object value)
        {
            _logger.Warn(value);
        }
    }
}
