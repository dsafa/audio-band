using System.ServiceModel;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    public static class LogManager
    {
        private static readonly ILoggerService LoggerService;

        static LogManager()
        {
            LoggerService = new ChannelFactory<ILoggerService>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.LoggerEndpoint)).CreateChannel();
        }

        public static IAudioSourceLogger GetAudioSourceLogger(string name)
        {
            return new AudioSourceLogger(name, LoggerService);
        }

        public static HostLogger GetHostLogger()
        {
            return new HostLogger(LoggerService);
        }
    }
}
