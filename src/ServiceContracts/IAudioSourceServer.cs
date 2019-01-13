using System;
using System.ServiceModel;

namespace ServiceContracts
{
    /// <summary>
    /// Contract for a server where a host can use to initiate communication with.
    /// </summary>
    [ServiceContract]
    public interface IAudioSourceServer
    {
        /// <summary>
        /// Register a new <see cref="IAudioSourceHost"/> server at the endpoint.
        /// </summary>
        /// <param name="hostServiceUri">Endpoint for the <see cref="IAudioSourceHost"/> service.</param>
        /// <param name="audioSourceDirectory">Directory of the audio source.</param>
        /// <returns>True if successfully registered.</returns>
        [OperationContract]
        bool RegisterHost(Uri hostServiceUri, string audioSourceDirectory);
    }
}
