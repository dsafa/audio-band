using System;

namespace ServiceContracts
{
    public static class ServiceHelper
    {
        public static Uri BaseEndpoint => new Uri("net.pipe://localhost/audioband");
        public static Uri ServerEndpoint => new Uri(BaseEndpoint, "server");
        public static Uri LoggerEndpoint => new Uri(BaseEndpoint, "logging");
        
        public static Uri GetAudioSourceEndpoint(string name)
        {
            return new Uri(BaseEndpoint, $"audiosources/{name}");
        }
    }
}
