using System;
using System.Net.NetworkInformation;

namespace Hoff.Core.Services.Common.Interfaces.Wireless
{
    public interface IAdhoc : IDisposable
    {
        WirelessAPConfiguration LoadConfig(IWifiSettings settings);
        bool Setup();
    }
}