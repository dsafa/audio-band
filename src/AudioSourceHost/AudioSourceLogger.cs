using AudioBand.AudioSource;
using NLog;

namespace AudioSourceHost
{
    public class AudioSourceLogger : IAudioSourceLogger
    {
        private readonly string _name;
        private readonly ILogger _logger;

        public AudioSourceLogger(string audiosourceName)
        {
            _logger = LogManager.GetLogger($"AudioSource({audiosourceName})");
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(object value)
        {
            _logger.Debug(value);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(object value)
        {
            _logger.Error(value);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(object value)
        {
            _logger.Info(value);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(object value)
        {
            _logger.Warn(value);
        }
    }
}
