using System;

namespace AudioSourceHost
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Missing directory name");
                return;
            }

            var directory = args[0];
            var host = new Host(directory);
            host.Initialize();

            // Keep program alive
            System.Windows.Forms.Application.Run();
        }
    }
}
