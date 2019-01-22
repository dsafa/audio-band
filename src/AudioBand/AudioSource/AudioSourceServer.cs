using System;
using System.ServiceModel;
using AudioBand.ServiceContracts;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// The implementation of <see cref="IAudioSourceServer"/>.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    internal class AudioSourceServer : IAudioSourceServer
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ServiceHost _audioSourceServerHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceServer"/> class
        /// with the directory of the audio source.
        /// </summary>
        /// <param name="audioSourceDirectory">The directory of the audio source.</param>
        public AudioSourceServer(string audioSourceDirectory)
        {
            Endpoint = ServiceHelper.GetAudioSourceServerEndpoint(audioSourceDirectory);

            _audioSourceServerHost = new ServiceHost(this);
            _audioSourceServerHost.AddServiceEndpoint(typeof(IAudioSourceServer), new NetNamedPipeBinding(), Endpoint);
            _audioSourceServerHost.Open();
        }

        /// <inheritdoc/>
        public event EventHandler<Uri> HostRegistered;

        /// <inheritdoc/>
        public Uri Endpoint { get; private set; }

        /// <inheritdoc/>
        public void IsAlive()
        {
        }

        /// <inheritdoc/>
        public bool RegisterHost(Uri hostServiceUri)
        {
            HostRegistered?.Invoke(this, hostServiceUri);

            Logger.Debug($"Audio source registered at {hostServiceUri}");
            return true;
        }
    }
}
