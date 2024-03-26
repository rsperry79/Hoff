using System;
using System.Device.Wifi;
using System.Net.NetworkInformation;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig.Ap;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig
{
    public class ApConfig : IApConfig, IDisposable
    {
        private bool _dispose = false;

        private static ILogger Logger;
        private static IWifiSettings wifiSettings;
        private static IAdhoc Adhoc;
        private static IApClient Client;
        private static ISettingsService Settings;

        private static IServiceProvider ServiceProvider;

        public ApConfig(ISettingsService settings, IServiceProvider serviceProvider)
        {
            try
            {
                Logger = this.GetCurrentClassLogger();

                Settings = settings;

                ServiceProvider = serviceProvider;
                wifiSettings = (IWifiSettings)Settings.Get(typeof(IWifiSettings));
                NetworkHelpers.Setup();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public bool StartAndWaitForConfig()
        {
            bool adhoc = wifiSettings.IsAdHoc;

            if (adhoc)
            {
                bool adHocresult = LoadAdhoc();
                return adHocresult;
            }
            else
            {
                WifiAvailableNetwork network = GetScannedNetwork();
                bool clientResult = LoadApClient(network);
                return clientResult;
            }
        }

        private static WifiAvailableNetwork GetScannedNetwork()
        {
            WifiAvailableNetwork network = null;

            IWifiEnvironment wifiEnvironment = (IWifiEnvironment)ServiceProvider.GetService(typeof(IWifiEnvironment));
            Array aps = wifiEnvironment.GetAvailableAPs();
            foreach (WifiAvailableNetwork net in aps)
            {
                if (net.Ssid == wifiSettings.SSID)
                {
                    network = net;
                }
            }

            return network;
        }

        /// <summary>
        /// Configure and enable the Wireless station interface
        /// </summary>
        /// <param name="settings"><![CDATA[IWifiSettings]]>></param>
        /// <returns></returns>
        public bool SetConfiguration(IWifiSettings settings)
        {
            bool result = false;
            if (settings.IsAdHoc)
            {
                WirelessAPConfiguration wconf = Adhoc.LoadConfig(settings);
                if (wconf != null) { result = true; }
            }
            else
            {
                Wireless80211Configuration wconf = Client.LoadConfig(settings);
                if (wconf != null) { result = true; }
            }

            return result;
        }

        private static bool LoadAdhoc()
        {
            try
            {
                Adhoc = new AdHoc(wifiSettings);

                if (Adhoc.Setup() == false)
                {
                    // Reboot device to Activate Access Point on restart
                    Logger.LogInformation($"Setup Soft AP, Rebooting device");
                    Power.RebootDevice();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        private static bool LoadApClient(WifiAvailableNetwork network)
        {
            if (Adhoc != null)
            {
                Logger.LogInformation("Disposing Adhoc");
                Adhoc.Dispose();
            }

            Client = new ApClient(wifiSettings, network);
            bool result = Client.Setup();

            return result;
        }

        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._dispose)
            {
                return;
            }

            if (disposing)
            {
                Adhoc?.Dispose();
            }

            this._dispose = true;
        }
    }
}
