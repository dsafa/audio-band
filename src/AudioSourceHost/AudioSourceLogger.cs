using System.ServiceModel;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    public class AudioSourceLogger : IAudioSourceLogger
    {
        private readonly string _name;
        private readonly ILoggerService _loggerService;

        public AudioSourceLogger(string audiosourceName)
        {
            _name = "AudioSource:" + audiosourceName;
            _loggerService = new ChannelFactory<ILoggerService>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.LoggerEndpoint)).CreateChannel();
        }

        public void Debug(string message)
        {
            _loggerService.Debug(_name, message);
        }

        public void Debug(object value)
        {
            _loggerService.Debug(_name, value?.ToString());
        }

        public void Error(string message)
        {
            _loggerService.Error(_name, message);
        }

        public void Error(object value)
        {
            _loggerService.Error(_name, value?.ToString());
        }

        public void Info(string message)
        {
            _loggerService.Info(_name, message);
        }

        public void Info(object value)
        {
            _loggerService.Info(_name, value?.ToString());
        }

        public void Warn(string message)
        {
            _loggerService.Warn(_name, message);
        }

        public void Warn(object value)
        {
            _loggerService.Warn(_name, value?.ToString());
        }
    }
}
