using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Server.ApHelper.Ap;
using Hoff.Server.ApHelper.Models;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace Hoff.Server.ApHelper
{
    public class ApConfig : IApConfig
    {
        public static WifiSettings WifiSettings;

        // Connected Station count
        private static string url;

        private static DhcpServer dhcpserver;
        private static DebugLogger Logger;

        public static IPAddress address = new(new byte[] { 192, 168, 4, 1 });
        public static IPAddress mask = new(new byte[] { 255, 255, 255, 0 });

        private static Wireless80211Configuration Config;

        public ApConfig(DebugLogger logger, IPAddress serverIp = null, IPAddress serverMask = null)
        {
            Logger = logger;

            if (serverIp != null)
            {
                address = serverIp;
            }

            if (serverMask != null)
            {
                mask = serverMask;
            }

            url = $"http://{address}";
        }

        public bool StartAndWaitForConfig()
        {

            if (!Wireless80211.IsEnabled())
            {
                Logger.LogInformation($"Wireless80211.IsEnabled: {Wireless80211.IsEnabled()} ");

                dhcpserver = new DhcpServer
                {
                    CaptivePortalUrl = url
                };

                Logger.LogInformation($"CaptivePortalUrl: {url}");
                _ = dhcpserver.Start(address, mask);
            }

            NetworkChange.NetworkAPStationChanged += NetworkChange_NetworkAPStationChanged;
            bool result = CheckForConnection();

            return result;
        }

        public static bool SetConfiguration(string ssid, string password)
        {
            Wireless80211Configuration current = Wireless80211.GetConfiguration();

            if (current.Ssid != ssid)
            {
                if (current.Password != password && password != null)
                {
                    _ = Wireless80211.Configure(ssid, password);
                    dhcpserver.Stop();
                    WirelessAP.Disable();
                    Thread.Sleep(200);


                    Power.RebootDevice();
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public static void GetAvailableAPs()
        {
            WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

            // Set up the AvailableNetworksChanged event to pick up when scan has completed
            wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;

            wifi.ScanAsync();


            // give it some time to perform the initial "connect"
            // trying to scan while the device is still in the connect procedure will throw an exception
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }


        ~ApConfig()
        {
            dhcpserver?.Dispose();
        }

        private static bool CheckForConnection()
        {
            bool wasSetup = false;

            bool enabled = Wireless80211.IsEnabled();
            if (enabled)
            {
                bool success = GetConnection();
                if (success)
                {
                    wasSetup = ValidateConnection();
                }
            }

            return wasSetup;
        }

        private static bool ValidateConnection()
        {
            string IpAdr = Wireless80211.WaitIP();
            Logger.LogInformation($"Connected as {IpAdr}");
            // We can even wait for a DateTime now
            Thread.Sleep(100);
            bool ready = WifiNetworkHelper.Status == NetworkHelperStatus.NetworkIsReady;
            if (ready)
            {
                if (DateTime.UtcNow.Year > DateTime.MinValue.Year)
                {
                    Logger.LogInformation($"We have a valid UTC date: {DateTime.UtcNow}");
                    Logger.LogInformation($"Exiting SoftAp");
                    return true;
                }
                else
                {
                    Logger.LogInformation($"We have a invalid date!!! ( {DateTime.UtcNow} )");
                    Logger.LogInformation($"Exiting SoftAp");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool GetConnection()
        {
            try
            {
                Logger.LogInformation($"Running in normal mode, connecting to Access point");
                Config = GetConf();
                GetAvailableAPs();
                bool success;
                // For devices like STM32, the password can't be read
                if (string.IsNullOrEmpty(Config.Password))
                {
                    // In this case, we will let the automatic connection happen
                    success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
                }
                else
                {
                    // If we have access to the password, we will force the reconnection
                    // This is mainly for ESP32 which will connect normally like that.
                    success = WifiNetworkHelper.ConnectDhcp(Config.Ssid, Config.Password, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
                }

                return success;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        private static Wireless80211Configuration GetConf()
        {
            Wireless80211Configuration conf = Wireless80211.GetConfiguration();
            WifiSettings = new WifiSettings();
            return conf;
        }

        /// <summary>
        /// Event handler for Stations connecting or Disconnecting
        /// </summary>
        /// <param name="NetworkIndex">The index of Network Interface raising event</param>
        /// <param name="e">Event argument</param>
        private static void NetworkChange_NetworkAPStationChanged(int NetworkIndex, NetworkAPStationEventArgs e)
        {
            Logger.LogInformation($"NetworkAPStationChanged event Index:{NetworkIndex} Connected:{e.IsConnected} Station:{e.StationIndex} ");

            // if connected then get information on the connecting station
            if (e.IsConnected)
            {
                WirelessAPConfiguration wapconf = WirelessAPConfiguration.GetAllWirelessAPConfigurations()[0];
                WirelessAPStation station = wapconf.GetConnectedStations(e.StationIndex);

                string macString = BitConverter.ToString(station.MacAddress);
                Logger.LogInformation($"Station mac {macString} RSSI:{station.Rssi} PhyMode:{station.PhyModes} ");
            }
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
            Logger.LogInformation(
                $"Net SSID :{current.Ssid},  " +
                $"BSSID : {current.Bsid}, " +
                $"RSSI : {current.NetworkRssiInDecibelMilliwatts}, " +
                $"Signal : {current.SignalBars}");


            WifiSettings.APsAvailable = availableNetworks;
        }
    }
}
