using System;
using System.ServiceModel;
using System.Timers;
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
        private Timer _pingTimer = new Timer(1000) { AutoReset = false };

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

            _pingTimer.Elapsed += PingTimerOnElapsed;
            _pingTimer.Start();
        }

        private async void PingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _audioSourceServer.IsAlive();
                _pingTimer.Start();
            }
            catch (Exception)
            {
                _logger.Error("Unable to ping main audio source server");
                await _instance.Close();
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
