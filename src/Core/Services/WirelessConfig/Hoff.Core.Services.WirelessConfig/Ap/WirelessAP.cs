using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig.Ap
{
    public class WirelessAP : IWirelessAP
    {
        private static ILogger Logger; // the logging interface
        private static DhcpServer dhcpServer; // the DHCH server
        private static IWifiSettings wifiSettings; // the wifi settings being used.

        public WirelessAP(IWifiSettings settings, ILoggerCore loggerCore)
        {
            Logger = loggerCore.GetDebugLogger(this.GetType().Name.ToString());
            wifiSettings = settings;
        }

        /// <summary>
        /// Set-up the Wireless AP settings, enable and save
        /// </summary>
        /// <returns>True if already set-up</returns>
        public bool Setup()
        {
            try
            {
                NetworkInterface ni = NetworkHelpers.GetInterface();
                WirelessAPConfiguration wapconf = NetworkHelpers.GetConfiguration();

                // Check if already Enabled and return true
                if (wapconf.Options == (WirelessAPConfiguration.ConfigurationOptions.Enable | WirelessAPConfiguration.ConfigurationOptions.AutoStart) && ni.IPv4Address == wifiSettings.Address.ToString())
                {
                    if (NetworkHelpers.IsAdHoc())
                    {
                        wifiSettings.IsAdHoc = true;
                        return this.EnableDhcp();
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
                    this.SaveAndDeauth(wapconf);
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Checks to see if the device is connected to the Access Point
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public bool CheckForConnection(bool isEnabled)
        {
            bool wasSetup = false;
            if (isEnabled)
            {
                bool success = this.GetConnection();
                if (success)
                {
                    wasSetup = this.ValidateConnection();
                }
            }

            return wasSetup;
        }

        /// <summary>
        /// Gets all available Access Points
        /// </summary>
        public void GetAvailableAPs()
        {
            try
            {
                WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += this.Wifi_AvailableNetworksChanged;
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

        /// <summary>
        /// Disable the Soft AP for next restart.
        /// </summary>
        public void Disable()
        {
            dhcpServer.Stop();
            WirelessAPConfiguration wapconf = NetworkHelpers.GetConfiguration();
            wapconf.Options = WirelessAPConfiguration.ConfigurationOptions.None;
            NetworkChange.NetworkAPStationChanged += this.NetworkChange_NetworkAPStationChanged;
            this.SaveAndDeauth(wapconf);
        }

        /// <summary>
        /// Enables the DHCP server for AdHoc connections.
        /// </summary>
        /// <returns></returns>
        private bool EnableDhcp()
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

        /// <summary>
        /// Saves the Config, disconnects stations and reboots.
        /// </summary>
        /// <param name="wapconf"></param>
        private void SaveAndDeauth(WirelessAPConfiguration wapconf)
        {
            wapconf.SaveConfiguration();
            wapconf.DeAuthStation(0);
            Power.RebootDevice();
        }

        /// <summary>
        /// Validates if the connection is correct.
        /// </summary>
        /// <returns></returns>
        private bool ValidateConnection()
        {
            IPAddress IpAdr = NetworkHelpers.GetAndWaitForIP();
            Logger.LogTrace($"Connected with IP {IpAdr}");
            // We can even wait for a DateTime now
            Thread.Sleep(100);
            bool ready = WifiNetworkHelper.Status == NetworkHelperStatus.NetworkIsReady;
            if (ready)
            {
                if (DateTime.UtcNow.Year > DateTime.MinValue.Year)
                {
                    Logger.LogTrace($"We have a valid UTC date: {DateTime.UtcNow}");
                    Logger.LogTrace($"Exiting SoftAp");
                    return true;
                }
                else
                {
                    Logger.LogTrace($"We have a invalid date!!! ( {DateTime.UtcNow} )");
                    Logger.LogTrace($"Exiting SoftAp");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the connection to the Access Point configured in the settings.
        /// </summary>
        /// <returns>If it connected</returns>
        private bool GetConnection()
        {
            try
            {
                Logger.LogTrace($"Running in normal mode, connecting to Access point");
                Wireless80211Configuration Config = NetworkHelpers.GetAllConfiguration();
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

        /// <summary>
        /// Event handler for Stations connecting or Disconnecting
        /// </summary>
        /// <param name="NetworkIndex">The index of Network Interface raising event</param>
        /// <param name="e">Event argument</param>
        private void NetworkChange_NetworkAPStationChanged(int NetworkIndex, NetworkAPStationEventArgs e)
        {
            Logger.LogTrace($"NetworkAPStationChanged event Index:{NetworkIndex} Connected:{e.IsConnected} Station:{e.StationIndex} ");

            // if connected then get information on the connecting station
            if (e.IsConnected)
            {
                WirelessAPConfiguration wapconf = WirelessAPConfiguration.GetAllWirelessAPConfigurations()[0];
                WirelessAPStation station = wapconf.GetConnectedStations(e.StationIndex);

                string macString = BitConverter.ToString(station.MacAddress);
                Logger.LogTrace($"Station mac {macString} RSSI:{station.Rssi} PhyMode:{station.PhyModes} ");
            }
        }

        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            try
            {
                WifiAvailableNetwork[] availableNetworks = sender.NetworkReport.AvailableNetworks;
                if (availableNetworks.Length > 0)
                {

                    Logger.LogTrace($"Available Networks: {availableNetworks.Length}");

                    int index = availableNetworks.Length >= 1 ? availableNetworks.Length - 1 : 0;
                    WifiAvailableNetwork current = availableNetworks[index];

                    Logger.LogTrace(
                        $"Net SSID :{current.Ssid},  "
                        +
                        $"BSSID : {current.Bsid}, " +
                        $"RSSI : {current.NetworkRssiInDecibelMilliwatts}, " +
                        $"Signal : {current.SignalBars}");

                    wifiSettings.APsAvailable = availableNetworks;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
