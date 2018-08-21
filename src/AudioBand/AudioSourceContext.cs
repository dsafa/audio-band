using AudioBand.AudioSource;

namespace AudioBand
{
    internal class AudioSourceContext : IAudioSourceContext
    {
        public IAudioSourceLogger Logger { get; }

        public AudioSourceContext(string connectorName)
        {
            Logger = new AudioSourceLogger(connectorName);
        }
    }
}
