using System;
using System.ServiceModel;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IAudioSourceServer
    {
        [OperationContract]
        Uri RegisterAudioSource(string name, Uri hostUri);
    }
}
