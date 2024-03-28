using Hoff.Core.IntegrationTests.Integration.Tests.Models;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Tests.Common;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Core.IntegrationTests.Integration.Tests.Helpers
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
            _ = serviceCollection
                .AddTransient(typeof(DiLoggingTestClass));
        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection

                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiDefaults));
        }
    }
}
