using System.Device.Wifi;
using System.Net;

namespace Hoff.Core.Hardware.Common.Interfaces.Services
{
    public interface IWifiSettings
    {
        IPAddress Address { get; set; }
        WifiAvailableNetwork[] APsAvailable { get; set; }
        IPAddress NetMask { get; set; }
        string Password { get; }
        string SSID { get; }

        bool IsAdhoc { get; set; }
        bool IsStaticIP { get; set; }
    }
}