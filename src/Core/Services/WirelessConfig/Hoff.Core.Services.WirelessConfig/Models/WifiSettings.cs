using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;

using Hoff.Core.Hardware.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

namespace Hoff.Core.Services.WirelessConfig.Models
{
    public class WifiSettings : SettingsStorageItem, IWifiSettings, IChangeNotification
    {
        private static WirelessAPConfiguration Configuration { get; set; }
        public WifiAvailableNetwork[] APsAvailable { get; set; }

        public IPAddress Address { get; set; } = IPAddress.Parse("192.168.4.1");

        public IPAddress NetMask { get; set; } = IPAddress.Parse("255.255.255.0");

        public bool IsStaticIP { get; set; } = false;
        public string SSID
        {
            get
            {
                this.Initialize();

                return Configuration.Ssid;
            }
        }

        public string Password
        {
            get
            {
                this.Initialize();
                return Configuration.Password;
            }
        }

        public bool IsAdhoc { get; set; } = false;

        protected void Initialize()
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
