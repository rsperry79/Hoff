using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Services.WirelessConfig.Models
{
    public class WifiSettings : IWifiSettings
    {
        private static DebugLogger Logger;
        private static WirelessAPConfiguration Configuration { get; set; }
        public WifiAvailableNetwork[] APsAvailable { get; set; }

        public IPAddress Address { get; set; } = IPAddress.Parse("192.168.4.1");

        public IPAddress NetMask { get; set; } = IPAddress.Parse("255.255.255.0");

        public bool IsStaticIP { get; set; } = false;
        public string SSID
        {
            get
            {
                GetConfig();

                return Configuration.Ssid;
            }
        }

        public string Password
        {
            get
            {
                GetConfig();
                return Configuration.Password;
            }
        }

        public bool IsAdhoc { get; set; } = false;

        private static void GetConfig()
        {
            try
            {
                Configuration ??= NetworkHelpers.GetConfiguration();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        public WifiSettings(ILoggerCore loggerCore) => Logger = loggerCore.GetDebugLogger(this.GetType().Name.ToString());
    }
}
