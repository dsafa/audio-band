using System.Collections.ObjectModel;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    public interface IAudioSourceManager
    {
        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        ObservableCollection<IInternalAudioSource> AudioSources { get; }

        /// <summary>
        /// Load all audio sources.
        /// </summary>
        void LoadAudioSources();
    }
}
