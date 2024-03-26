using System;

using Hoff.Core.Services.Common.Interfaces.Wireless;

namespace Hoff.Core.Services.Common.Interfaces.Services
{
    public interface IApConfig : IDisposable
    {

        bool SetConfiguration(IWifiSettings settings);
        bool StartAndWaitForConfig();
    }
}