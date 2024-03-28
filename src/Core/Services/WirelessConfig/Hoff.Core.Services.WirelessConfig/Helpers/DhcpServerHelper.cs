
using System;
using System.Net;

using Hoff.Core.Services.Common.Interfaces.Wireless;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig.Helpers
{
    internal class DhcpServerHelper : IDhcpServerHelper, IDisposable
    {
        private static DhcpServer dhcpServer;
        private static ILogger Logger;

        public DhcpServerHelper() => Logger = this.GetCurrentClassLogger();

        /// <summary>
        /// Enables the DHCP server for AdHoc connections.
        /// </summary>
        /// <returns></returns>
        public void EnableDhcp(IWifiSettings wifiSettings)
        {
            string url = $"http://{wifiSettings.Address}";

            Logger.LogInformation($"Dhcp Server Addr: {wifiSettings.Address}");
            Logger.LogInformation($"Dhcp Server Mask: {wifiSettings.NetMask}");

            dhcpServer = new DhcpServer
            {
                CaptivePortalUrl = url
            };

            bool dhcpInitResult = dhcpServer.Start(IPAddress.Parse(wifiSettings.Address), IPAddress.Parse(wifiSettings.NetMask), 1200);

            if (!dhcpInitResult)
            {
                Logger.LogInformation($"Error initializing DHCP server. \r\nRebooting...");
                // This happens after a very freshly flashed device
                Power.RebootDevice();
            }
            else
            {
                Logger.LogInformation("Dhcp Server started");
            }

            Logger.LogInformation($"CaptivePortalUrl: {url}");
        }

        public void DisableDhcp()
        {
            dhcpServer.Stop();
        }

        public void Dispose()
        {
            this.DisableDhcp();
            dhcpServer.Dispose();
        }
    }
}
