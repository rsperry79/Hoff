

using System.Net;
using System.Threading;

using Hoff.Core.Services.Logging;
using Hoff.Server.ApHelper;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Server.Core
{
    public class Program
    {

        private static DebugLogger Logger;

        private static ApConfig ApHelper;

        private static readonly IPAddress address = new(new byte[] { 192, 168, 4, 1 });
        private static readonly IPAddress mask = new(new byte[] { 255, 255, 255, 0 });

        public static void Main()
        {
            LoggerCore loggerCore = new();
            const string loggerName = "Core_Web_Server";
            const LogLevel minLogLevel = LogLevel.Trace;
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

            ApHelper = new ApConfig(Logger, address, mask);
            _ = ApHelper.StartAndWaitForConfig();

            _ = new Web.Server(Logger);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
