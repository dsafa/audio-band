using System;
using NLog;

namespace AudioBand
{
    internal class ConnectorLogger : Connector.ILogger
    {
        private readonly NLog.ILogger _logger;

        public ConnectorLogger(string name)
        {
            _logger = LogManager.GetLogger("Connector:" + name);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
