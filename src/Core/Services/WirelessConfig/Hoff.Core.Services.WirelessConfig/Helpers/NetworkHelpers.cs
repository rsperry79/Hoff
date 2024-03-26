using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces.Wireless;

using Microsoft.Extensions.Logging;

using nanoFramework.Networking;

namespace Hoff.Core.Services.WirelessConfig.Helpers
{
    internal static class NetworkHelpers
    {
        private static bool _isSetup = false;
        internal static void Setup()
        {
            if (!_isSetup)
            {
                WifiNetworkHelper.SetupNetworkHelper();
                _isSetup = true;
            }
        }

        internal static NetworkHelperStatus WaitForWifi(ILogger logger, int timeoutSeconds = 30)
        {
            NetworkHelperStatus status;
            NetworkHelperStatus isReady = WifiNetworkHelper.Status;
            int count = 0;
            do
            {
                status = WifiNetworkHelper.Status;

                if (status != isReady)
                {
                    Thread.Sleep(100);
                    count += 100;
                    if (count % 1000 == 0)
                    {
                        NetworkHelperStatus displayStatus = status;
                        logger.LogInformation($"Waiting for Network to be ready. Status is: {displayStatus}");
                    }
                }
            } while (status != isReady && count < timeoutSeconds * 1000);
            NetworkInterface conf = GetInterface();
            logger.LogInformation($"WaitForWifi Status: {status} IP: {conf.IPv4Address} Mask: {conf.IPv4SubnetMask}");
            return status;
        }

        internal static NetworkInterface GetInterface()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Find WirelessAP interface
            foreach (NetworkInterface ni in Interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    return ni;
                }
            }

            return null;
        }

        internal static void SetStaticIP(IWifiSettings settings)
        {
            string ip = settings.Address.ToString();
            string netmask = settings.NetMask.ToString();
            string gateway = settings.Gateway == null ? settings.Address.ToString() : settings.Gateway.ToString();
            GetInterface().EnableStaticIPv4(ip, netmask, gateway);
        }
    }
}
