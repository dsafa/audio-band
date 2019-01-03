using System;

namespace ServiceContracts
{
    public class ServiceHelper
    {
        Uri BaseEndpoint => new Uri("net.pipe://localhost/audioband");
        Uri AudioBandEndpoint => new Uri(BaseEndpoint, "update");
        Uri LoggerEndpoint => new Uri(BaseEndpoint, "logging");
        
        public Uri GetAudioSourceEndpoint(string name)
        {
            return new Uri(BaseEndpoint, $"audiosources/{name}");
        }
    }
}
