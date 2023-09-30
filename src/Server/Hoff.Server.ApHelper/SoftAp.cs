
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Server.ApHelper.Ap;

using Iot.Device.DhcpServer;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace Hoff.Server.ApHelper
{
    public class SoftAp
    {

        // Connected Station count
        private static int connectedCount = 0;
        private static string url;

        private static SoftApWebServer server;
        private static DhcpServer dhcpserver;
        private static DebugLogger Logger;

        private static readonly IPAddress address = new(new byte[] { 192, 168, 4, 1 });
        private static readonly IPAddress mask = new(new byte[] { 255, 255, 255, 0 });

        public SoftAp(DebugLogger logger)
        {
            Logger = logger;
            server = new SoftApWebServer(Logger);

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
                LoadSoftAp();
            }

            bool result = CheckAndWaitForConnection();

            Logger.LogInformation("exiting wait loop.");
            return result;
        }

        ~SoftAp()
        {
            dhcpserver.Dispose();
        }

        private static bool CheckAndWaitForConnection()
        {
            bool wasSetup = false;
            do
            {
                bool enabled = Wireless80211.IsEnabled();
                if (enabled)
                {
                    bool success = GetConnection();
                    if (success)
                    {
                        wasSetup = ValidateConnection();
                    }
                }

                Thread.Sleep(1000);
            } while (!wasSetup);

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
                Wireless80211Configuration conf = Wireless80211.GetConfiguration();
                bool success;
                // For devices like STM32, the password can't be read
                if (string.IsNullOrEmpty(conf.Password))
                {
                    // In this case, we will let the automatic connection happen
                    success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
                }
                else
                {
                    // If we have access to the password, we will force the reconnection
                    // This is mainly for ESP32 which will connect normally like that.
                    success = WifiNetworkHelper.ConnectDhcp(conf.Ssid, conf.Password, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
                }

                return success;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        private static void LoadSoftAp()
        {
            Wireless80211.Disable();
            if (WirelessAP.Setup() == false)
            {
                // Reboot device to Activate Access Point on restart
                Logger.LogInformation($"Setup Soft AP, Rebooting device");
                Power.RebootDevice();
            }

            server.Start();
            Logger.LogInformation("Running Soft AP, waiting for client to connect");

            NetworkChange.NetworkAPStationChanged += NetworkChange_NetworkAPStationChanged;
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

                connectedCount++;

                // Start web server when it connects otherwise the bind to network will fail as
                // no connected network. Start web server when first station connects
                if (connectedCount == 1)
                {
                    // Wait for Station to be fully connected before starting web server
                    // other you will get a Network error
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
