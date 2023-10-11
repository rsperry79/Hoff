namespace Hoff.Core.Hardware.Common.Interfaces.Services
{
    public interface IApConfig
    {
        IWifiSettings GetWifiSettings();
        bool SetConfiguration(IWifiSettings settings);
        bool StartAndWaitForConfig();
    }
}