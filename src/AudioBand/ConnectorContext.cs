using AudioBand.Connector;

namespace AudioBand
{
    internal class ConnectorContext : IConnectorContext
    {
        public IConnectorLogger Logger { get; }

        public ConnectorContext(string connectorName)
        {
            Logger = new ConnectorLogger(connectorName);
        }
    }
}
