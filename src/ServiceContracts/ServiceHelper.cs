using System;

namespace ServiceContracts
{
    public static class ServiceHelper
    {
        public static Uri BaseEndpoint => new Uri("net.pipe://localhost/audioband/");
        public static Uri AudioSourceServerEndpoint => new Uri(BaseEndpoint, "audiosource-server");
        
        public static Uri GetAudioSourceHostEndpoint(string name)
        {
            return new Uri(BaseEndpoint, $"audiosources/{name}/host");
        }
    }
}
