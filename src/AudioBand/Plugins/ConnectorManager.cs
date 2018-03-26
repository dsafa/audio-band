using System;
using AudioBand.Connector;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace AudioBand.Plugins
{
    internal class ConnectorManager
    {
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable <IAudioConnector> AudioConnectors { get; private set; }

        public event EventHandler PluginsChanged;

        private const string PluginFolderName = "connectors";
        private AggregateCatalog _catalog;
        private CompositionContainer _container;
        private List<DirectoryCatalog> _directoryCatalogs;
        private FileSystemWatcher _fileSystemWatcher;

        public ConnectorManager()
        {
            BuildCatalog();
            BuildContainer();
            AudioConnectors = _container.GetExportedValues<IAudioConnector>();
        }

        private void BuildCatalog()
        {
            var basePath = DirectoryHelper.BaseDirectory;
            var pluginFolderPath = Path.Combine(basePath, PluginFolderName);
            if (!Directory.Exists(pluginFolderPath))
            {
                Directory.CreateDirectory(pluginFolderPath);
            }

            _directoryCatalogs = Directory.EnumerateDirectories(pluginFolderPath, "*", SearchOption.TopDirectoryOnly)
                .Select(d => new DirectoryCatalog(d))
                .ToList();

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
            foreach (var directoryCatalog in _directoryCatalogs)
            {
                directoryCatalog.Refresh();
            }

            AudioConnectors = _container.GetExportedValues<IAudioConnector>();
            PluginsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BuildContainer()
        {
            _container =  new CompositionContainer(_catalog, CompositionOptions.IsThreadSafe);
        }
    }
}
