using System;
using System.Device.Wifi;
using System.Net;
using System.Net.NetworkInformation;

using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Helpers;

using Microsoft.Extensions.Logging;

namespace Hoff.Core.Services.WirelessConfig.Models
{
    public class WifiSettings : SettingsStorageItem, IWifiSettings
    {
        protected static WirelessAPConfiguration Configuration { get; set; }
        public WifiAvailableNetwork[] APsAvailable { get; set; }

        public IPAddress Address { get; set; } = IPAddress.Parse("192.168.4.1");

        public IPAddress NetMask { get; set; } = IPAddress.Parse("255.255.255.0");

        public bool IsStaticIP { get; set; } = false;
        public string SSID { get; set; }

        public string Password { get; set; }

        public bool IsAdHoc { get; set; } = false;

        protected override void Initialize()
        {
            try
            {
                Configuration ??= NetworkHelpers.GetConfiguration();

                this.SSID = Configuration.Ssid;
                this.Password = Configuration.Password;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        public WifiSettings(ILoggerCore loggerCore)
        {
            this.Initialize();
            Logger = loggerCore.GetDebugLogger(this.GetType().Name.ToString());
        }
    }
}
