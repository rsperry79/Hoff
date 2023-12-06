using System.Device.Wifi;
using System.Net.NetworkInformation;

using Hoff.Server.ApHelper.Ap;

namespace Hoff.Server.ApHelper.Models
{
    public class WifiSettings
    {
        public WifiAvailableNetwork[] APsAvailable { get; set; }


        public string SSID
        {
            get
            {
                Wireless80211Configuration conf = Wireless80211.GetConfiguration();
                return conf.Ssid;
            }
        }

        public string Password
        {
            get
            {
                Wireless80211Configuration conf = Wireless80211.GetConfiguration();
                return conf.Password;
            }
        }
    }
}
