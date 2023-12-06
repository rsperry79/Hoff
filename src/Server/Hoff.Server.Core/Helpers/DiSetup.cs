using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Core.Services.WirelessConfig.Ap;
using Hoff.Core.Services.WirelessConfig.Models;
using Hoff.Server.Web;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Server.Core.Helpers
{
    internal class DiSetup
    {
        public static ServiceProvider Services;

        private static readonly ServiceCollection serviceCollection = new();

        internal static ServiceProvider ConfigureServices()
        {
            LoadSingleTons();
            LoadTransients();
            Services = serviceCollection.BuildServiceProvider();
            return Services;
        }

        internal static void LoadTransients()
        {
            _ = serviceCollection
                .AddTransient(typeof(UiServer))
                .AddTransient(typeof(ISettingsStorageItem), typeof(SettingsStorageItem));
        }

        internal static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(NvsStorage))
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings))
                .AddSingleton(typeof(IWirelessAP), typeof(WirelessAP))
                .AddSingleton(typeof(ISettingsStorage), typeof(SettingsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService));

        }
    }
}
