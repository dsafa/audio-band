using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using AudioBand.Logging;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    public class AudioSourceManager : IAudioSourceManager
    {
        private const string PluginFolderName = "AudioSources";
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioSourceManager>();

        /// <inheritdoc />
        public async Task<IEnumerable<IInternalAudioSource>> LoadAudioSourcesAsync()
        {
            Logger.Debug("Loading audio sources in {path}", PluginFolderPath);
            if (!Directory.Exists(PluginFolderPath))
            {
                Logger.Debug("Audiosource folder does not exist");
                return Enumerable.Empty<IInternalAudioSource>();
            }

            var audioSources = new List<IInternalAudioSource>();
            foreach (var dir in Directory.EnumerateDirectories(PluginFolderPath))
            {
                try
                {
                    var proxy = await CreateProxy(dir);
                    audioSources.Add(proxy);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error creating proxy for audiosource in {path}", dir);
                }
            }

            return audioSources;
        }

        private async Task<AudioSourceProxy> CreateProxy(string dir)
        {
            return await Task.Run(() => new AudioSourceProxy(dir));
        }
    }
}
