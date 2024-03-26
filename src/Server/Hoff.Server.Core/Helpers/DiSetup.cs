using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Server.Web;
using Hoff.Services.Common.Interfaces.Storage;

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
                .AddTransient(typeof(UiServer));

        }

        internal static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))

                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))

                .AddSingleton(typeof(IWifiSettings), typeof(WifiDefault))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment));

        }
    }
}
