using System;
using System.ServiceModel;
using NLog;
using AudioBand.ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// The implementation of <see cref="IAudioSourceServer"/>.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    internal class AudioSourceServer : IAudioSourceServer
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Occurs when a AudioSourceHost registers to this server.
        /// </summary>
        public event EventHandler<AudioSourceRegisteredEventArgs> HostRegistered;

        /// <inheritdoc/>
        public void IsAlive()
        {
        }

        /// <inheritdoc/>
        public bool RegisterHost(Uri hostServiceUri)
        {
            HostRegistered?.Invoke(this, new AudioSourceRegisteredEventArgs(hostServiceUri));

            Logger.Debug($"Audio source registered at {hostServiceUri}");
            return true;
        }
    }
}
