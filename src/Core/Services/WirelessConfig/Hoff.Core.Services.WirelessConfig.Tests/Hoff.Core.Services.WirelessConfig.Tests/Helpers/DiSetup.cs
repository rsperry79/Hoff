using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Core.Services.WirelessConfig.Models;
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
            serviceCollection = serviceCollection
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment));

        }
    }
}
