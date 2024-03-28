
using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Networking;

namespace Hoff.Core.Services.WirelessConfig.Ap
{
    internal class ApClient : IApClient, IDisposable
    {
        private static ILogger Logger;
        private static WifiAdapter Wifi;
        public ApClient(IWifiSettings settings, WifiAvailableNetwork network = null)
        {
            Logger = this.GetCurrentClassLogger();
            Wifi = WifiAdapter.FindAllAdapters()[0];
            bool matches = ConfigMatches(settings);
            if (!matches)
            {
                _ = this.LoadConfig(settings);
            }

            if (WifiNetworkHelper.Status != NetworkHelperStatus.Started)
            {
                _ = ConnectToWifi(settings, network);
            }
        }

        /// <summary>
        /// Checks and loads the configuration if it is not already set.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Wireless80211Configuration LoadConfig(IWifiSettings settings)
        {
            Wireless80211Configuration conf = GetClientConfiguration();

            conf.Ssid = settings.SSID;
            conf.Password = settings.Password;
            conf.Authentication = (AuthenticationType)settings.AuthenticationType;
            conf.Encryption = (EncryptionType)settings.EncryptionType;
            conf.Radio = (RadioType)settings.RadioType;

            if (settings.IsStaticIP)
            {
                NetworkHelpers.SetStaticIP(settings);
            }
            else
            {
                NetworkHelpers.GetInterface().EnableDhcp();
            }

            conf.Options = Wireless80211Configuration.ConfigurationOptions.Enable | Wireless80211Configuration.ConfigurationOptions.AutoConnect;
            conf.SaveConfiguration();

            return conf;
        }

        /// <summary>
        /// Validates if the connection is correct.
        /// </summary>
        /// <returns></returns>
        public bool Setup()
        {
            IPAddress IpAdr = GetAndWaitForIP();
            Logger.LogInformation($"Connected with IP {IpAdr}");
            NetworkHelperStatus status = NetworkHelpers.WaitForWifi(Logger);

            if (status == NetworkHelperStatus.NetworkIsReady)
            {
                if (DateTime.UtcNow.Year > DateTime.MinValue.Year)
                {
                    return true;
                }
                else
                {
                    Logger.LogCritical($"We have a invalid date!!! ( {DateTime.UtcNow} )");
                    return false;
                }
            }
            else
            {
                Logger.LogCritical($"Network is not ready: {status}");
                return false;
            }
        }

        private static bool ConnectToWifi(IWifiSettings settings, WifiAvailableNetwork network)
        {
            bool success = false;
            // if the AP scan found a matching network, then connect to it
            if (network != null)
            {
                _ = Wifi.Connect(network, WifiReconnectionKind.Automatic, settings.Password);
            }
            else if (string.IsNullOrEmpty(settings.Password))
            {
                // For devices like STM32, the password can't be read
                success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
            }
            else
            {
                // for most devices we can use the password to connect if the network is not found via the scan.
                success = WifiNetworkHelper.ConnectDhcp(settings.SSID, settings.Password, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
            }

            return success;
        }

        private static Wireless80211Configuration GetClientConfiguration()
        {
            NetworkInterface ni = NetworkHelpers.GetInterface();
            return Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
        }

        private static bool ConfigMatches(IWifiSettings settings)
        {
            Wireless80211Configuration conf = GetClientConfiguration();
            bool matches = conf.Ssid == settings.SSID && conf.Password == settings.Password;
            return matches;
        }

        private static IPAddress GetAndWaitForIP()
        {

            bool hasIP = false;
            int count = 0;
            while (!hasIP | count < 10)
            {

                string iPAddress = NetworkHelpers.GetInterface().IPv4Address;
                if (!string.IsNullOrEmpty(iPAddress))
                {
                    if (iPAddress[0] != '0')
                    {
                        IPAddress ip = IPAddress.Parse(iPAddress);
                        return ip;
                    }
                }
                else
                {
                    count++;
                    Thread.Sleep(500);
                }
            }

            return null;
        }

        public void Dispose()
        {

        }

        ~ApClient()
        {
            this.Dispose();
        }

    }
}
