using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NLog;
using NLog.Config;

namespace AudioSourceHost
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }

            LogManager.ThrowExceptions = true;
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(basePath, "nlog.config"));

            var directory = args[0];
            var endpointAddress = args[1];

            try
            {
                var host = new Host();
                host.Initialize(directory, endpointAddress);

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Error(e, $"Error with initialization, directory:{directory}, endpoint:{endpointAddress}");
            }
        }

        public static void Exit()
        {
            QuitEvent.Set();
        }
    }
}
