
using System;
using System.Net.NetworkInformation;

namespace Hoff.Core.Services.Common.Interfaces.Wireless
{
    public interface IApClient : IDisposable
    {
        Wireless80211Configuration LoadConfig(IWifiSettings settings);
        bool Setup();
    }
}
