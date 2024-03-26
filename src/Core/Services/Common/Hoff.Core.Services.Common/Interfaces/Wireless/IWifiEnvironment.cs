
using System;

namespace Hoff.Core.Services.Common.Interfaces.Wireless
{
    public interface IWifiEnvironment
    {
        Array GetAvailableAPs(bool forceUpdate = false, int scanTimeInSeconds = 30);
    }
}
