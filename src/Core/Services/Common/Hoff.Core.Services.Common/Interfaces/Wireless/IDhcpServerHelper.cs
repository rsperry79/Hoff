
using System;

namespace Hoff.Core.Services.Common.Interfaces.Wireless
{
    public interface IDhcpServerHelper : IDisposable
    {
        void DisableDhcp();
        void EnableDhcp(IWifiSettings wifiSettings);
    }
}
