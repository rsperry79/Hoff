namespace Hoff.Core.Services.Common.Interfaces.Wireless
{
    public interface IWifiSettings
    {
        string Address { get; set; }

        int AuthenticationType { get; set; }
        string NetMask { get; set; }
        string Password { get; set; }
        string SSID { get; set; }

        bool IsAdHoc { get; set; }
        bool IsStaticIP { get; set; }
        int EncryptionType { get; set; }
        int RadioType { get; set; }
        string Gateway { get; set; }
    }
}