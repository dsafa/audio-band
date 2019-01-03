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

            try
            {
                var host = new Host();
                host.Initialize(directory, endpointAddress);

                // Keep program alive
                System.Windows.Forms.Application.Run();
            }
            catch (Exception e)
            {
                LogManager.GetHostLogger().Error($"Error with initialization, directory:{directory}, endpoint:{endpointAddress}, Error: {e}");
            }
        }
    }
}
