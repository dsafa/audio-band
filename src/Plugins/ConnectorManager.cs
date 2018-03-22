using AudioBand.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace AudioBand.Plugins
{
    internal class ConnectorManager
    {
        [ImportMany]
        public IEnumerable <IAudioConnector> AudioConnectors { get; private set; }

        private const string PluginFolderName = "connectors";
        private DirectoryCatalog _catalog;
        private CompositionContainer _container;

        public ConnectorManager()
        {
            _catalog = BuildCatalog();
            _container = BuildContainer();
            AudioConnectors = _container.GetExportedValues<IAudioConnector>();
        }

        private DirectoryCatalog BuildCatalog()
        {
            var basePath = DirectoryHelper.BaseDirectory;
            var pluginFolderPath = Path.Combine(basePath, PluginFolderName);
            if (!Directory.Exists(pluginFolderPath))
            {
                Directory.CreateDirectory(pluginFolderPath);
            }

            return new DirectoryCatalog(pluginFolderPath);
        }

        private CompositionContainer BuildContainer()
        {
            return new CompositionContainer(_catalog);
        }
    }
}
