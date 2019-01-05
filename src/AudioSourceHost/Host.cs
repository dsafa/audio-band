using System;
using System.ServiceModel;
using AudioBand.AudioSource;
using NLog;
using ServiceContracts;

namespace AudioSourceHost
{
    internal class Host
    {
        private ILogger _logger;
        private AudioSourceHostService _instance;
        private IAudioSource _audioSource;
        private ServiceHost _serviceHost;
        private IAudioSourceServer _audioSourceServer;

        public void Initialize(string audioSourceDirectory)
        {
            _audioSource = AudioSourceLoader.LoadFromDirectory(audioSourceDirectory);
            _audioSource.Logger = new AudioSourceLogger(_audioSource.Name);

            _logger = LogManager.GetLogger($"Host({_audioSource.Name})");

            var hostEndpoint = ServiceHelper.GetAudioSourceHostEndpoint(_audioSource.Name);
            _instance = new AudioSourceHostService(_audioSource);
            _serviceHost = new ServiceHost(_instance);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceHost), new NetNamedPipeBinding(), hostEndpoint);
            _serviceHost.Closed += ServiceHostOnClosed;
            _serviceHost.Open();

            _logger.Debug($"Connecting to audio source server to register {_audioSource.Name}");
            var serverBinding = new NetNamedPipeBinding()
            {
                SendTimeout = TimeSpan.FromSeconds(10),
                ReceiveTimeout = TimeSpan.FromSeconds(10),
            };

            _audioSourceServer = new ChannelFactory<IAudioSourceServer>(serverBinding, new EndpointAddress(ServiceHelper.AudioSourceServerEndpoint)).CreateChannel();
            var success = _audioSourceServer.RegisterHost(hostEndpoint);
            if (!success)
            {
                _logger.Warn($"Unable to regiester audio source {_audioSource.Name}");
                Program.Exit();
            }
        }

        private async void ServiceHostOnClosed(object sender, System.EventArgs e)
        {
            _logger.Debug("Client closed connection");
            _serviceHost.Close();
            await _instance.Close();
            Program.Exit();
        }
    }
}
