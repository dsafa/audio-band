using System;
using System.Threading;

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
                LogManager.GetHostLogger().Error($"Error with initialization, directory:{directory}, endpoint:{endpointAddress}, Error: {e}");
            }
        }

        public static void Exit()
        {
            QuitEvent.Set();
        }
    }
}
