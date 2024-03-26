using Hoff.Core.Services.Common.Interfaces.Wireless;

namespace Hoff.Core.Services.WirelessConfig.Models
{
    public class WifiSettings : IWifiSettings
    {
        public string Address { get; set; } = "192.168.4.1";

        public string NetMask { get; set; } = "255.255.255.0";

        public string Gateway { get; set; } = "192.168.4.1";

        public bool IsStaticIP { get; set; } = false;
        public string SSID { get; set; }

        public string Password { get; set; }

        public bool IsAdHoc { get; set; } = true;
        public int AuthenticationType { get; set; } = (int)System.Net.NetworkInformation.AuthenticationType.WPA2;
        public int EncryptionType { get; set; } = (int)System.Net.NetworkInformation.EncryptionType.WPA2_PSK;
        public int RadioType { get; set; } = (int)System.Net.NetworkInformation.RadioType._802_11n;

    }
}
