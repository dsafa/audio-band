using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    public interface IAudioSourceManager
    {
        /// <summary>
        /// Load all audio sources.
        /// </summary>
        /// <returns>The initial list of audio sources.</returns>
        Task<IEnumerable<IInternalAudioSource>> LoadAudioSourcesAsync();
    }
}
