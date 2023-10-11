using System;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Ap;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig
{
    public class ApConfig : IApConfig
    {
        private static IWifiSettings wifiSettings;
        private static ILoggerCore LoggerCore;
        private static DebugLogger Logger;

        public ApConfig(ILoggerCore loggerCore, IWifiSettings settings)
        {
            LoggerCore = loggerCore;
            Logger = loggerCore.GetDebugLogger(this.GetType().Name.ToString());
            wifiSettings = settings;
        }

        public IWifiSettings GetWifiSettings()
        {
            return wifiSettings;
        }

        public bool StartAndWaitForConfig()
        {
            bool result = false;

            try
            {
                bool wifiIsEnabled = NetworkHelpers.HasSSID();
                Logger.LogTrace($"NetworkHelpers.IsEnabled: {wifiIsEnabled}");

                if (!wifiIsEnabled)
                {
                    result = WirelessAP.Setup(wifiSettings, LoggerCore);
                }
                else
                {
                    result = this.CheckForConnection(wifiIsEnabled);
                }

                NetworkChange.NetworkAPStationChanged += NetworkChange_NetworkAPStationChanged;

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        public bool SetConfiguration(IWifiSettings settings)
        {
            WirelessAPConfiguration current = NetworkHelpers.GetConfiguration();

            if (current.Ssid != settings.SSID)
            {
                if (current.Password != settings.Password && settings.Password != null)
                {
                    _ = NetworkHelpers.Configure(settings.SSID, settings.Password);

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

        private bool CheckForConnection(bool isEnabled)
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
        /// Validates if the connection is correct.
        /// </summary>
        /// <returns></returns>
        private bool ValidateConnection()
        {
            string IpAdr = NetworkHelpers.GetAndWaitForIP();
            Logger.LogTrace($"Connected as {IpAdr}");
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
                Logger.LogInformation($"Running in normal mode, connecting to Access point");
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
        private static void NetworkChange_NetworkAPStationChanged(int NetworkIndex, NetworkAPStationEventArgs e)
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

    }
}
