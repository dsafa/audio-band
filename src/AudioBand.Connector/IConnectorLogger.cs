﻿namespace AudioBand.Connector
{
    public interface IConnectorLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}
