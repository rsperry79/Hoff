using System.Net;
using System.Threading;

using Hoff.Core.Services.Logging;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Server.Core
{
    public class Program
    {
        private static DhcpServer dhcpserver;
        private static DebugLogger Logger;

        private static readonly IPAddress address = new(new byte[] { 192, 168, 4, 1 });
        private static readonly IPAddress mask = new(new byte[] { 255, 255, 255, 0 });
        private static string url;

        public static void Main()
        {
            LoggerCore loggerCore = new();
            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
            url = $"http://{address}";

            dhcpserver = new DhcpServer
            {
                CaptivePortalUrl = url
            };

            Logger.LogInformation($"CaptivePortalUrl: {url}");
            _ = dhcpserver.Start(address, mask);

            //SoftAp softAp = new SoftAp(Logger);
            //softAp.StartAndWaitForConfig();

            //The webapp url
            Logger.LogInformation($"http://{IPAddress.GetDefaultLocalAddress()}/");
            _ = new
            Web.Server(Logger);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
