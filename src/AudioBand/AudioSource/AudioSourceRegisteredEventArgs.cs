using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Event args for a <see cref="AudioSourceServer.HostRegistered"/> event.
    /// </summary>
    internal class AudioSourceRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceRegisteredEventArgs"/> class
        /// with the uri of the host.
        /// </summary>
        /// <param name="hostServiceUri">The <see cref="Uri"/> of the host.</param>
        public AudioSourceRegisteredEventArgs(Uri hostServiceUri)
        {
            HostServiceUri = hostServiceUri;
        }

        /// <summary>
        /// Gets the uri of the host.
        /// </summary>
        public Uri HostServiceUri { get; }
    }
}
