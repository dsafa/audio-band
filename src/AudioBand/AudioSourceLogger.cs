using AudioBand.AudioSource;
using NLog;

namespace AudioBand
{
    internal class AudioSourceLogger : IAudioSourceLogger
    {
        private readonly ILogger _logger;

        public AudioSourceLogger(string name)
        {
            _logger = LogManager.GetLogger("AudioSource:" + name);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
