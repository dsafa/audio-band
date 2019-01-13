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
            var serverBinding = new NetNamedPipeBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };

            _serviceHost = new ServiceHost(_instance);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceHost), serverBinding, hostEndpoint);
            _serviceHost.Closed += ServiceHostOnClosed;
            _serviceHost.Open();

            _logger.Debug($"Connecting to audio source server to register {_audioSource.Name}");
            _audioSourceServer = new ChannelFactory<IAudioSourceServer>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.AudioSourceServerEndpoint)).CreateChannel();
            var success = _audioSourceServer.RegisterHost(hostEndpoint, audioSourceDirectory);
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
