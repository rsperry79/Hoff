using System.Device.Wifi;
using System.Net;

namespace Hoff.Core.Services.Common.Interfaces.Services
{
    public interface IWifiSettings
    {
        IPAddress Address { get; set; }
        WifiAvailableNetwork[] APsAvailable { get; set; }
        IPAddress NetMask { get; set; }
        string Password { get; set; }
        string SSID { get; set; }

        bool IsAdHoc { get; set; }
        bool IsStaticIP { get; set; }
    }
}