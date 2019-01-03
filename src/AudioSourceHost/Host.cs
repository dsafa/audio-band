using System;
using System.ServiceModel;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    internal class Host
    {
        private IAudioSource _audioSource;
        private AudioSourceHostService _hostService;

        public void Initialize(string audioSourceDirectory, string hostEndpoint)
        {
            _audioSource = AudioSourceLoader.LoadFromDirectory(audioSourceDirectory);
            _audioSource.Logger = new AudioSourceLogger(_audioSource.Name);

            _hostService = new AudioSourceHostService(_audioSource, hostEndpoint);
        }
    }
}
