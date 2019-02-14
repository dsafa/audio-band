using System.IO;
using System.Reflection;

namespace AudioBand
{
    /// <summary>
    /// Provides directory helper functions.
    /// </summary>
    internal static class DirectoryHelper
    {
        /// <summary>
        /// Gets the base directory of the assembly.
        /// </summary>
        /// <remarks>Since this assembly is loaded by explorer we use this to get the directory.</remarks>
        public static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
