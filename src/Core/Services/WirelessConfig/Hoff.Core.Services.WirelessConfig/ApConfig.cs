using System;
using System.Net.NetworkInformation;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.Runtime.Native;

namespace Hoff.Core.Services.WirelessConfig
{
    public class ApConfig : IApConfig
    {
        private static IWifiSettings wifiSettings;
        private static DebugLogger Logger;
        private static IWirelessAP WirelessAP;

        public ApConfig(ILoggerCore loggerCore, IWifiSettings settings, IWirelessAP wirelessAP)
        {
            Logger = loggerCore.GetDebugLogger(this.GetType().Name.ToString());
            wifiSettings = settings;
            WirelessAP = wirelessAP;
        }

        /// <summary>
        /// Returns the current WifiSettings
        /// </summary>
        /// <returns>WifiSettings</returns>
        public IWifiSettings GetWifiSettings()
        {
            return wifiSettings;
        }

        public bool StartAndWaitForConfig()
        {
            try
            {
                bool wifiIsEnabled = NetworkHelpers.HasSSID();
                Logger.LogTrace($"NetworkHelpers.IsEnabled: {wifiIsEnabled}");

                bool result = !wifiIsEnabled ? WirelessAP.Setup() : WirelessAP.CheckForConnection(wifiIsEnabled);
                WirelessAP.GetAvailableAPs();

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
            bool result;
            if (current.Ssid != settings.SSID)
            {
                if (current.Password != settings.Password && settings.Password != null)
                {
                    _ = NetworkHelpers.Configure(settings.SSID, settings.Password);

                    WirelessAP.Disable();
                    Thread.Sleep(200);

                    Power.RebootDevice();
                }

                result = false;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
