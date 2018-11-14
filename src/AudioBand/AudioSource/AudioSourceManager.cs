using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using NLog;

namespace AudioBand.AudioSource
{
    internal class AudioSourceManager
    {
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable <IAudioSource> AudioSources { get; private set; }

        public event EventHandler AudioSourcesChanged;

        private const string PluginFolderName = "AudioSources";
        private AggregateCatalog _catalog;
        private CompositionContainer _container;
        private List<DirectoryCatalog> _directoryCatalogs;
        private FileSystemWatcher _fileSystemWatcher;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public AudioSourceManager()
        {
            BuildCatalog();
            BuildContainer();
            AudioSources = _container.GetExportedValues<IAudioSource>();

            foreach (var audioSource in AudioSources)
            {
                audioSource.Logger = new AudioSourceLogger(audioSource.Name);
                Logger.Debug($"Audio source loaded: `{audioSource.Name}`");
            }
        }

        private void BuildCatalog()
        {
            var basePath = DirectoryHelper.BaseDirectory;
            var pluginFolderPath = Path.Combine(basePath, PluginFolderName);
            Logger.Debug($"Searching for audio sources in path `{pluginFolderPath}`");

            if (!Directory.Exists(pluginFolderPath))
            {
                Directory.CreateDirectory(pluginFolderPath);
            }

            _directoryCatalogs = Directory.EnumerateDirectories(pluginFolderPath, "*", SearchOption.TopDirectoryOnly)
                .Select(d => new DirectoryCatalog(d))
                .ToList();
            _directoryCatalogs.ForEach(d => Logger.Debug($"Found subfolder {d.Path}"));

            _catalog = new AggregateCatalog(_directoryCatalogs);

            _fileSystemWatcher = new FileSystemWatcher(pluginFolderPath)
            {
                EnableRaisingEvents =  true,
                IncludeSubdirectories = true,
                Filter = "*.dll"
            };
            _fileSystemWatcher.Created += FileSystemWatcherOnCreated;
        }

        private void FileSystemWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Logger.Debug($"Detected new audio source folder `{fileSystemEventArgs.FullPath}");
            foreach (var directoryCatalog in _directoryCatalogs)
            {
                directoryCatalog.Refresh();
            }

            AudioSources = _container.GetExportedValues<IAudioSource>();
            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BuildContainer()
        {
            _container =  new CompositionContainer(_catalog, CompositionOptions.IsThreadSafe);
        }
    }
}
