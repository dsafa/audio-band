using System.ServiceModel;
using AudioBand.AudioSource;
using NLog;
using ServiceContracts;

namespace AudioSourceHost
{
    internal class Host
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private AudioSourceHostService _instance;
        private IAudioSource _audioSource;
        private ServiceHost _serviceHost;

        public void Initialize(string audioSourceDirectory)
        {
            _audioSource = AudioSourceLoader.LoadFromDirectory(audioSourceDirectory);
            _audioSource.Logger = new AudioSourceLogger(_audioSource.Name);

            var hostEndpoint = ServiceHelper.GetAudioSourceHostEndpoint(_audioSource.Name);
            _instance = new AudioSourceHostService(_audioSource);
            _serviceHost = new ServiceHost(_instance);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceHost), new NetNamedPipeBinding(), hostEndpoint);
            _serviceHost.Faulted += ServiceHostFaulted;
            _serviceHost.Closed += ServiceHostClosed;
            _serviceHost.Open();

            Logger.Debug($"Connecting to audio source server to register {_audioSource.Name}");
            var server = new ChannelFactory<IAudioSourceServer>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.AudioSourceServerEndpoint))
                .CreateChannel();
            var success = server.RegisterHost(hostEndpoint);
            if (!success)
            {
                Logger.Warn($"Unable to regiester audio source {_audioSource.Name}");
            }
        }

        private void ServiceHostClosed(object sender, System.EventArgs e)
        {
            Program.Exit();
        }

        private void ServiceHostFaulted(object sender, System.EventArgs e)
        {
            Logger.Error($"Service faulted. {_audioSource.Name}");
        }
    }
}
