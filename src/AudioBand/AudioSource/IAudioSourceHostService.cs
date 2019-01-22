using System;
using System.Runtime.CompilerServices;
using AudioBand.ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Represents a service that provides access to audio source host functionality
    /// </summary>
    internal interface IAudioSourceHostService
    {
        /// <summary>
        /// Occurs when the host restarts.
        /// </summary>
        event EventHandler Restarted;

        /// <summary>
        /// Gets the callback associated with the host.
        /// </summary>
        AudioSourceHostCallback HostCallback { get; }

        /// <summary>
        /// Gets the audio source host.
        /// </summary>
        /// <param name="caller">Caller</param>
        /// <returns>The audio source host.</returns>
        IAudioSourceHost GetHost([CallerMemberName]string caller = "");

        /// <summary>
        /// Restart the service.
        /// </summary>
        void Restart();
    }
}
