using System;

namespace ServiceContracts
{
    /// <summary>
    /// Helper methods for services.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Gets the base endpoint for services.
        /// </summary>
        public static Uri BaseEndpoint => new Uri("net.pipe://localhost/audioband/");

        /// <summary>
        /// Gets the endpoint for the audio source server.
        /// </summary>
        public static Uri GetAudioSourceServerEndpoint(string name) => new Uri(BaseEndpoint, $"audiosource-server/{name}");
        
        /// <summary>
        /// Creates an endpoint for a <see cref="IAudioSourceHost"/> for the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Uri GetAudioSourceHostEndpoint(string name)
        {
            return new Uri(BaseEndpoint, $"audiosources/{name}/host");
        }
    }
}
