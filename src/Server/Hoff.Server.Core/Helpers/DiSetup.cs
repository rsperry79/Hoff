using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Core.Services.WirelessConfig.Ap;
using Hoff.Core.Services.WirelessConfig.Models;
using Hoff.Server.Web;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
namespace Hoff.Server.Core.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings))
                .AddSingleton(typeof(IWirelessAP), typeof(WirelessAP))
                .AddTransient(typeof(UiServer))
                .BuildServiceProvider();
        }
    }
}
