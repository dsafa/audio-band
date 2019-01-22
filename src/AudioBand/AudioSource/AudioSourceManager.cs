using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceManager
    {
        private const string PluginFolderName = "AudioSources";
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _audioSourcesLock = new object();

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        public ObservableCollection<IInternalAudioSource> AudioSources { get; private set; } = new ObservableCollection<IInternalAudioSource>();

        /// <summary>
        /// Load all audio sources.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadAudioSources()
        {
            Logger.Debug("Searching for orphan processes");
            var processes = Process.GetProcessesByName("AudioSourceHost");
            foreach (var p in processes)
            {
                Logger.Debug("Found orphan process");
                p.Kill();
                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            Logger.Debug("Loading audio sources");
            var tasks = Directory.EnumerateDirectories(PluginFolderPath).Select(dir => AudioSourceProxy.CreateProxy(dir)).ToArray();

            var proxies = await Task.WhenAll(tasks).ConfigureAwait(false);
            foreach (var proxy in proxies.Where(p => p != null))
            {
                AudioSources.Add(proxy);
            }
        }

        /// <summary>
        /// Close all services.
        /// </summary>
        public void Close()
        {
            foreach (var proxy in AudioSources.Cast<AudioSourceProxy>())
            {
                proxy.Close();
            }
        }
    }
}
