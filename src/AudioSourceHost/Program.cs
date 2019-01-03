using System;

namespace AudioSourceHost
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }

            var directory = args[0];
            var endpointAddress = args[1];

            var host = new Host();
            host.Initialize(directory, endpointAddress);

            // Keep program alive
            System.Windows.Forms.Application.Run();
        }
    }
}
