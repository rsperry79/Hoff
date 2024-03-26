using System.Threading;

using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Settings;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Server.Web;
using Hoff.Services.Common.Interfaces.Storage;
using Hoff.Tests.Common;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Tests.App.Helpers
{
    public class ProgramHelpers
    {
        public static ServiceProvider Services;

        private static IServiceCollection serviceCollection;

        public static void ConfigureServices()
        {
            serviceCollection = TestHelpers.GetServiceCollection();
            LoadSingleTons();
            LoadTransients();

        }

        private static void LoadTransients()
        {
            _ = serviceCollection
                .AddTransient(typeof(UiServer));
        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))

                .AddSingleton(typeof(IWifiSettings), typeof(WifiDefault))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment));
        }

        public static void InfiniteLoop()
        {

            Thread.Sleep(Timeout.Infinite);
        }


    }
}
