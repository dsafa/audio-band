using ServiceContracts;

namespace AudioSourceHost
{
    public class HostLogger
    {
        private readonly ILoggerService _loggerService;

        public HostLogger(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public void Debug(string message)
        {
            _loggerService.Debug("AudioSourceHost", message);
        }

        public void Error(string message)
        {
            _loggerService.Error("AudioSourceHost", message);
        }
    }
}
