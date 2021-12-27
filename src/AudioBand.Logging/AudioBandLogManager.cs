using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace AudioBand.Logging
{
    /// <summary>
    /// Log manager for audioband loggers.
    /// </summary>
    public static class AudioBandLogManager
    {
        private static readonly LogFactory LogFactory = new LogFactory();

        /// <summary>
        /// Gets a logger instance.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>A logger.</returns>
        public static ILogger GetLogger(string name)
        {
            return LogFactory.GetLogger(name);
        }

        /// <summary>
        /// Gets a logger instance.
        /// </summary>
        /// <typeparam name="T">The type used to name the logger.</typeparam>
        /// <returns>A logger.</returns>
        public static ILogger GetLogger<T>()
        {
            return LogFactory.GetLogger(typeof(T).FullName);
        }

        /// <summary>
        /// Initializes the log manager.
        /// </summary>
        public static void Initialize()
        {
            LayoutRenderer.Register<AudioBandExceptionLayoutRenderer>("audioband-exception");
            var configFileFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var config = new XmlLoggingConfiguration(Path.Combine(configFileFolder, "nlog.config"));
            LogFactory.Configuration = config;
        }
    }
}
