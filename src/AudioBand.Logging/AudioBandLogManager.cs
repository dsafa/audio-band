using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System.IO;
using System.Reflection;

namespace AudioBand.Logging
{
    /// <summary>
    /// Log mananger for audioband loggers
    /// </summary>
    public static class AudioBandLogManager
    {
        private static LogFactory LogFactory;

        public static ILogger GetLogger(string name)
        {
            return LogFactory.GetLogger(name);
        }

        public static ILogger GetLogger<T>()
        {
            return LogFactory.GetLogger(typeof(T).FullName);
        }

        public static void Initialize()
        {
            LayoutRenderer.Register<AudioBandExceptionLayoutRenderer>("audioband-exception");
            var configFileFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var config = new XmlLoggingConfiguration(Path.Combine(configFileFolder, "nlog.config"));
            LogFactory = new LogFactory(config);
        }
    }
}
