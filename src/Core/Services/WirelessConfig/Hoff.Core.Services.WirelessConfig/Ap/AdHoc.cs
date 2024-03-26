using System;
using System.Net.NetworkInformation;

using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig.Ap
{
    public class AdHoc : IAdhoc, IDisposable
    {
        private static ILogger Logger; // the logging interface
        private static IDhcpServerHelper dhcpServer; // the DHCP server
        private static IWifiSettings WifiSettings; // the wifi settings being used.

        private static WirelessAPConfiguration WapConfig; // the wireless AP configuration

        public AdHoc(IWifiSettings wifiSettings)
        {
            Logger = this.GetCurrentClassLogger();

            NetworkHelpers.Setup();
            WapConfig = GetAdhocConfiguration();
            WifiSettings = wifiSettings;


        }

        /// <summary>
        /// Set-up the Wireless AP settings, enable and save
        /// </summary>
        /// <returns>True if already set-up</returns>
        public bool Setup()
        {
            if (!AdhocConfigMatches(WifiSettings))
            {
                //DisableClientWifi();
                WapConfig = this.LoadConfig(WifiSettings);
                SaveAndDeauth();
                Power.RebootDevice();
                return false;
            }
            else
            {
                NetworkHelperStatus status = NetworkHelpers.WaitForWifi(Logger);
                //this.EnableDhcp();
                return true;
            }
        }

        /// <summary>
        /// Checks and loads the configuration if it is not already set.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public WirelessAPConfiguration LoadConfig(IWifiSettings settings)
        {


            WapConfig.Ssid = string.IsNullOrEmpty(settings.SSID) ? "" : settings.SSID;
            WapConfig.Password = string.IsNullOrEmpty(settings.Password) ? "" : settings.Password;
            WapConfig.Authentication = (AuthenticationType)settings.AuthenticationType;
            WapConfig.Encryption = (EncryptionType)settings.EncryptionType;
            WapConfig.Radio = (RadioType)settings.RadioType;
            if (settings.IsStaticIP)
            {
                NetworkHelpers.SetStaticIP(settings);
            }
            else
            {
                Logger.LogCritical("AdHoc requires a static IP");
                throw new Exception("AdHoc requires a static IP");
            }

            WapConfig.Options = WirelessAPConfiguration.ConfigurationOptions.AutoStart | WirelessAPConfiguration.ConfigurationOptions.Enable;
            WapConfig.SaveConfiguration();

            Logger.LogInformation($"SSID: {WapConfig.Ssid} PW: {WapConfig.Password}");
            return WapConfig;
        }

        private static bool AdhocConfigMatches(IWifiSettings settings)
        {

            bool matches = WapConfig.Ssid == settings.SSID && WapConfig.Password == settings.Password;
            return matches;
        }

        private static WirelessAPConfiguration GetAdhocConfiguration()
        {
            NetworkInterface ni = NetworkHelpers.GetInterface();
            return WirelessAPConfiguration.GetAllWirelessAPConfigurations()[ni.SpecificConfigId];
        }

        private void NetworkChange_NetworkAPStationChanged(int NetworkIndex, NetworkAPStationEventArgs e)
        {
            Logger.LogInformation($"NetworkAPStationChanged event Index:{NetworkIndex} Connected:{e.IsConnected} Station:{e.StationIndex} ");

            // if connected then get information on the connecting station
            if (e.IsConnected)
            {
                WirelessAPStation station = WapConfig.GetConnectedStations(e.StationIndex);
                string macString = BitConverter.ToString(station.MacAddress);
                Logger.LogInformation($"Station mac {macString} RSSI:{station.Rssi} PhyMode:{station.PhyModes} ");
            }
        }

        private void Disable()
        {
            dhcpServer.DisableDhcp();
            SaveAndDeauth();
        }

        private static void DisableClientWifi()
        {
            NetworkInterface ni = NetworkHelpers.GetInterface();
            Wireless80211Configuration wconf = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
            wconf.Options = Wireless80211Configuration.ConfigurationOptions.None;
            wconf.SaveConfiguration();
        }

        private void EnableDhcp()
        {
            dhcpServer = new DhcpServerHelper();
            dhcpServer.EnableDhcp(WifiSettings);
        }

        private static void SaveAndDeauth()
        {
            WapConfig.SaveConfiguration();
            WapConfig.DeAuthStation(0);
        }

        public void Dispose()
        {
            if (dhcpServer != null)
            {
                this.Disable();
                dhcpServer.Dispose();
            }
        }

        ~AdHoc()
        {
            this.Dispose();
        }
    }
}
