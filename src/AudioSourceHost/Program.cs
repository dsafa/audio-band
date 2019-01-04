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
            if (args.Length < 1)
            {
                return;
            }

            LogManager.ThrowExceptions = true;
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(basePath, "nlog.config"));

            AppDomain.CurrentDomain.UnhandledException += (o, e) => LogManager.GetCurrentClassLogger().Error(e.ExceptionObject as Exception, "Unhandled exception");

            var directory = args[0];

            try
            {
                var host = new Host();
                host.Initialize(directory);

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Error(e, $"Error with initialization, directory:{directory}");
            }
        }

        public static void Exit()
        {
            QuitEvent.Set();
        }
    }
}
