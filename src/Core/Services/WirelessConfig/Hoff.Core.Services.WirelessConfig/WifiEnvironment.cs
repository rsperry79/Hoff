
using System;
using System.Collections;
using System.Device.Wifi;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Networking;

namespace Hoff.Core.Services.WirelessConfig
{
    public class WifiEnvironment : IWifiEnvironment
    {
        private static ILogger Logger;

        private static WifiAdapter Wifi;
        private static ArrayList APsAvailable { get; set; } = new ArrayList();
        public WifiEnvironment()
        {
            Logger = this.GetCurrentClassLogger();
            NetworkHelpers.Setup();
            Wifi = WifiAdapter.FindAllAdapters()[0];
            Wifi.AvailableNetworksChanged += this.Wifi_AvailableNetworksChanged;
        }

        /// <summary>
        /// Gets all available Access Points
        /// </summary>
        public Array GetAvailableAPs(bool forceUpdate = false, int scanTimeInSeconds = 5)
        {
            APsAvailable ??= new ArrayList();

            if (APsAvailable.Count > 0 && !forceUpdate)
            {
                return APsAvailable.ToArray();
            }

            NetworkHelperStatus status = NetworkHelpers.WaitForWifi(Logger);
            Logger.LogTrace($"NetworkHelperStatus: {status}");

            try
            {
                Wifi.ScanAsync();

            }
            catch (Exception ex)
            {
                Logger.LogCritical($"Failure starting a scan operation: {ex.StackTrace}");
                throw;
            }

            if (APsAvailable.Count < 1)
            {
                Thread.Sleep(TimeSpan.FromSeconds(scanTimeInSeconds));
            }

            Wifi.AvailableNetworksChanged -= this.Wifi_AvailableNetworksChanged;

            return APsAvailable.ToArray();
        }

        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            WifiNetworkReport report = sender.NetworkReport;
            try
            {
                if (report != null && report.AvailableNetworks != null && report.AvailableNetworks.Length > 0)
                {
                    ParseNetworkReport(report);
                }
                else
                {
                    Logger.LogInformation("No networks found");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private static void ParseNetworkReport(WifiNetworkReport report)
        {
            foreach (WifiAvailableNetwork net in report.AvailableNetworks)
            {

                if (!APsAvailable.Contains(net))
                {
                    Logger.LogTrace($"SSID: {net.Ssid} RSSI: {net.NetworkRssiInDecibelMilliwatts}");
                    _ = APsAvailable.Add(net);
                }
                else
                {
                    int temp = APsAvailable.IndexOf(net);
                    WifiAvailableNetwork item = (WifiAvailableNetwork)APsAvailable[temp];
                    if (item.NetworkRssiInDecibelMilliwatts > net.NetworkRssiInDecibelMilliwatts)
                    {
                        APsAvailable[temp] = net;
                    }
                }
            }
        }
    }
}
