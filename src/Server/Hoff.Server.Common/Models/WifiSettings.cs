using System.Collections;

namespace Hoff.Server.Common.Models
{
    public class WifiSettings
    {
        public ArrayList APsAvailable { get; set; } = new ArrayList();

        public bool AdHocEnabled { get; set; }
        public bool IsConfigured { get; set; }

        public string SSID { get; set; }
        public string Password { get; set; }

        public WifiSettings(bool adHocEnabled, bool isConfigured, string ssid, string password, ArrayList aPsAvailable)
            : this(adHocEnabled, isConfigured, ssid, password) => this.APsAvailable = aPsAvailable;

        public WifiSettings(bool adHocEnabled, bool isConfigured, string ssid, string password)
        {
            this.AdHocEnabled = adHocEnabled;
            this.IsConfigured = isConfigured;
            this.SSID = ssid;
            this.Password = password;
        }
    }
}
