using System;
using System.ServiceModel;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IAudioSourceServer
    {
        [OperationContract]
        bool RegisterHost(Uri hostServiceUri);
    }
}
