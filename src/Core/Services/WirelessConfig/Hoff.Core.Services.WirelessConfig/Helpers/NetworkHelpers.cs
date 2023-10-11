using System.Net.NetworkInformation;
using System.Threading;

using nanoFramework.Networking;

namespace Hoff.Core.Services.WirelessConfig.Helpers
{
    internal static class NetworkHelpers
    {

        private static NetworkInterface Network { get; set; }

        internal static NetworkInterface GetInterface()
        {
            if (Network != null)
            {
                return Network;
            }

            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Find WirelessAP interface
            foreach (NetworkInterface ni in Interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    Network = ni;
                }
            }

            return Network;
        }

        /// <summary>
        /// Find the Wireless AP configuration
        /// </summary>
        /// <returns>Wireless AP configuration or NUll if not available</returns>
        internal static WirelessAPConfiguration GetConfiguration()
        {
            NetworkInterface ni = GetInterface();
            return WirelessAPConfiguration.GetAllWirelessAPConfigurations()[ni.SpecificConfigId];
        }

        /// <summary>
        /// Get the Wireless station configuration.
        /// </summary>
        /// <returns>Wireless80211Configuration object</returns>
        internal static Wireless80211Configuration GetAllConfiguration()
        {
            NetworkInterface ni = GetInterface();
            return Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
        }

        internal static bool IsAdHoc()
        {
            Wireless80211Configuration conf = GetAllConfiguration();
            return conf.Options.HasFlag(Wireless80211Configuration.ConfigurationOptions.Enable);
        }
        internal static bool HasSSID()
        {
            var conf = GetAllConfiguration();
            bool isNull = conf.Ssid != string.Empty ? true : false;

            conf.Options.HasFlag(Wireless80211Configuration.ConfigurationOptions.SmartConfig);



            return isNull;
        }

        /// <summary>
        /// Disable the Wireless station interface.
        /// </summary>
        internal static void Disable()
        {
            Wireless80211Configuration wconf = GetAllConfiguration();
            wconf.Options = Wireless80211Configuration.ConfigurationOptions.None;
            wconf.SaveConfiguration();
        }

        /// <summary>
        /// Configure and enable the Wireless station interface
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static bool Configure(string ssid, string password)
        {
            // And we have to force connect once here even for a short time
            _ = WifiNetworkHelper.ConnectDhcp(ssid, password, token: new CancellationTokenSource(10000).Token);
            Wireless80211Configuration wconf = GetAllConfiguration();
            wconf.Options = Wireless80211Configuration.ConfigurationOptions.AutoConnect | Wireless80211Configuration.ConfigurationOptions.Enable;
            wconf.SaveConfiguration();
            return true;
        }


        internal static string GetAndWaitForIP()
        {
            while (true)
            {
                NetworkInterface ni = GetInterface();
                if (!string.IsNullOrEmpty(ni.IPv4Address))
                {
                    if (ni.IPv4Address[0] != '0')
                    {
                        return ni.IPv4Address;
                    }
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Returns the IP address of the Soft AP
        /// </summary>
        /// <returns>IP address</returns>
        internal static string GetIP()
        {
            NetworkInterface ni = GetInterface();
            return ni.IPv4Address;
        }






    }
}
