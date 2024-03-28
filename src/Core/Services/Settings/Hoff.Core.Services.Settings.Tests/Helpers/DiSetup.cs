using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig.Models;
using Hoff.Services.Common.Interfaces.Storage;
using Hoff.Tests.Common;

using Microsoft.Extensions.DependencyInjection;


namespace Hoff.Core.Services.Settings.Tests.Helpers
{
    public static class DiSetup

    {
        private static IServiceCollection serviceCollection;

        public static void ConfigureServices()
        {
            serviceCollection = TestHelpers.GetServiceCollection();
            LoadSingleTons();
            LoadTransients();

        }

        private static void LoadTransients()
        {

        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings));

        }
    }
}
