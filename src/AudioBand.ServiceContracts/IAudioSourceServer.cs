using System;
using System.ServiceModel;

namespace AudioBand.ServiceContracts
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
        /// <returns>True if successfully registered.</returns>
        [OperationContract]
        bool RegisterHost(Uri hostServiceUri);

        /// <summary>
        /// Ping to check if the server is still alive
        /// </summary>
        [OperationContract]
        void IsAlive();

        event EventHandler<Uri> HostRegistered;

        Uri Endpoint { get; }
    }
}
