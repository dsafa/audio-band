using AudioBand.AudioSource;

namespace AudioBand
{
    internal class AudioSourceContext : IAudioSourceContext
    {
        public IAudioSourceLogger Logger { get; }

        public AudioSourceContext(string audioSourceName)
        {
            Logger = new AudioSourceLogger(audioSourceName);
        }
    }
}
