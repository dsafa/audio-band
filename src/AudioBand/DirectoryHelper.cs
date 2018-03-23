using System.IO;
using System.Reflection;

namespace AudioBand
{
    internal static class DirectoryHelper
    {
        // Since this assembly is loaded by explorer we use this to get the directory
        public static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
