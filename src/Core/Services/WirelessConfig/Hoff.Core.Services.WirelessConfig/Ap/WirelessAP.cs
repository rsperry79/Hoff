using System;
using System.Device.Wifi;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig.Ap
{
    internal class WirelessAP
    {

        private static ILogger Logger;
        private static DhcpServer dhcpServer;
        private static IWifiSettings wifiSettings;

        /// <summary>
        /// Disable the Soft AP for next restart.
        /// </summary>
        internal static void Disable()
        {
            dhcpServer.Stop();
            WirelessAPConfiguration wapconf = NetworkHelpers.GetConfiguration();
            wapconf.Options = WirelessAPConfiguration.ConfigurationOptions.None;

            SaveAndDeauth(wapconf);
        }

        /// <summary>
        /// Set-up the Wireless AP settings, enable and save
        /// </summary>
        /// <returns>True if already set-up</returns>
        internal static bool Setup(IWifiSettings settings, ILoggerCore loggerCore)
        {
            try
            {
                Logger = loggerCore.GetCurrentClassLogger();
                wifiSettings = settings;
                NetworkInterface ni = NetworkHelpers.GetInterface();
                WirelessAPConfiguration wapconf = NetworkHelpers.GetConfiguration();
                GetAvailableAPs();

                // Check if already Enabled and return true
                if (wapconf.Options == (WirelessAPConfiguration.ConfigurationOptions.Enable | WirelessAPConfiguration.ConfigurationOptions.AutoStart) && ni.IPv4Address == wifiSettings.Address.ToString())
                {
                    if (NetworkHelpers.IsAdHoc())
                    {
                        wifiSettings.IsAdhoc = true;
                        return EnableDhcp();
                    }

                    return true;
                }
                else // do setup
                {

                    // Set up IP address for Soft AP
                    ni.EnableStaticIPv4(wifiSettings.Address.ToString(), wifiSettings.NetMask.ToString(), wifiSettings.Address.ToString());

                    //
                    wapconf.Options =
                        WirelessAPConfiguration.ConfigurationOptions.AutoStart |
                        WirelessAPConfiguration.ConfigurationOptions.Enable;


                    // SSID for Access Point will use default  "nano_xxxxxx"

                    // Maximum number of simultaneous connections, reserves memory for connections
                    wapconf.MaxConnections = 3;

                    // To set-up Access point with no Authentication
                    wapconf.Authentication = AuthenticationType.Open;
                    Logger.LogTrace("Device is now in Ad Hoc Mode");

                    // Save the configuration so on restart Access point will be running.
                    SaveAndDeauth(wapconf);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        internal static bool EnableDhcp()
        {
            string url = $"http://{NetworkHelpers.GetIP()}/";

            Logger.LogTrace($"Dhcp Server Addr: {wifiSettings.Address}");
            Logger.LogTrace($"Dhcp Server Mask: {wifiSettings.NetMask}");

            dhcpServer = new DhcpServer { CaptivePortalUrl = url };
            bool result = dhcpServer.Start(wifiSettings.Address, wifiSettings.NetMask);

            Logger.LogTrace($"Dhcp Server started: {result}");
            Logger.LogTrace($"CaptivePortalUrl: {url}");

            return result;
        }

        private static void SaveAndDeauth(WirelessAPConfiguration wapconf)
        {
            wapconf.SaveConfiguration();
            wapconf.DeAuthStation(0);
            Power.RebootDevice();

        }


        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            WifiAvailableNetwork[] availableNetworks = sender.NetworkReport.AvailableNetworks;
            int idex = availableNetworks.Length >= 1 ? availableNetworks.Length - 1 : 0;
            WifiAvailableNetwork current = availableNetworks[idex];
            Logger.LogTrace(
                $"Net SSID :{current.Ssid},  " +
                $"BSSID : {current.Bsid}, " +
                $"RSSI : {current.NetworkRssiInDecibelMilliwatts}, " +
            $"Signal : {current.SignalBars}");

            wifiSettings.APsAvailable = availableNetworks;
        }



        private static void GetAvailableAPs()
        {
            try
            {
                WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;
                // give it some time to perform the initial "connect"
                // trying to scan while the device is still in the connect procedure will throw an exception
                Thread.Sleep(TimeSpan.FromSeconds(2));
                wifi.ScanAsync();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        ~WirelessAP()
        {
            if (dhcpServer != null)
            {
                dhcpServer.Stop();
                dhcpServer.Dispose();
            }
        }
    }
}
