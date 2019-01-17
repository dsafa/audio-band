using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using AudioBand.AudioSource;
using NLog;
using ServiceContracts;

namespace AudioSourceHost
{
    internal class Host
    {
        private readonly Timer _checkServerTimer = new Timer { AutoReset = false, Interval = TimeSpan.FromSeconds(5).TotalMilliseconds };
        private ILogger _logger;
        private AudioSourceHostService _instance;
        private IAudioSource _audioSource;
        private ServiceHost _serviceHost;
        private IAudioSourceServer _audioSourceServer;

        public void Initialize(string audioSourceDirectory, string audioSourceServerEndpoint)
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
            _audioSourceServer = new ChannelFactory<IAudioSourceServer>(new NetNamedPipeBinding(), new EndpointAddress(audioSourceServerEndpoint)).CreateChannel();
            var success = _audioSourceServer.RegisterHost(hostEndpoint);
            if (!success)
            {
                _logger.Warn($"Unable to regiester audio source {_audioSource.Name}");
                Program.Exit();
            }

            _checkServerTimer.Elapsed += CheckServerTimerOnElapsed;
            _checkServerTimer.Start();
        }

        private async void CheckServerTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _audioSourceServer.IsAlive();
                _checkServerTimer.Start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not ping server");
                await Exit();
            }
        }

        private async void ServiceHostOnClosed(object sender, EventArgs e)
        {
            _logger.Debug("Client closed connection");
            await Exit();
        }

        private async Task Exit()
        {
            _serviceHost.Close();
            await _instance.Close();
            Program.Exit();
        }
    }
}
