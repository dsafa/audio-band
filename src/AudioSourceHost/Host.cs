using AudioBand.AudioSource;

namespace AudioSourceHost
{
    internal class Host
    {
        private readonly string _directory;
        private IAudioSource _audioSource;
        private AudioSourceHostService _service;

        public Host(string audioSourceDirectory)
        {
            _directory = audioSourceDirectory;
        }

        public void Initialize()
        {
            _audioSource = AudioSourceLoader.LoadFromDirectory(_directory);
            _audioSource.Logger = new AudioSourceLogger(_audioSource.Name);
            _service = new AudioSourceHostService(_audioSource);
        }
    }
}
