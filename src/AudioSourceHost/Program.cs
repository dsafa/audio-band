using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;

namespace AudioSourceHost
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
            var audioSourceServerEndpoint = args[1];

            var hostProcesses = Process.GetProcessesByName("AudioSourceHost");

            // use arguments as an identifier
            var existingHost = hostProcesses.FirstOrDefault(h => h.StartInfo.Arguments == $"{directory} {audioSourceServerEndpoint}" && h.Id != Process.GetCurrentProcess().Id);
            if (existingHost != null)
            {
                Logger.Debug("Found existing audio source host");
                existingHost.Kill();
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            try
            {
                var host = new Host();
                host.Initialize(directory, audioSourceServerEndpoint);

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error with initialization, directory:{directory}");
            }
        }

        public static void Exit()
        {
            QuitEvent.Set();
        }
    }
}
