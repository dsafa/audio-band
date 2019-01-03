using NLog;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Implementation of the logger service
    /// </summary>
    internal class AudioSourceLogger : ILoggerService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        public void Debug(string name, string message)
        {
            Logger.Debug($"{name} - {message}");
        }

        /// <inheritdoc/>
        public void Error(string name, string message)
        {
            Logger.Error($"{name} - {message}");
        }

        /// <inheritdoc/>
        public void Info(string name, string message)
        {
            Logger.Info($"{name} - {message}");
        }

        /// <inheritdoc/>
        public void Warn(string name, string message)
        {
            Logger.Warn($"{name} - {message}");
        }
    }
}
