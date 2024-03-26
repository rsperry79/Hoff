using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Services.Common.Interfaces.Storage;
using Hoff.Tests.Common;
using Hoff.Tests.Common.Interfaces;
using Hoff.Tests.Common.Models;

using Microsoft.Extensions.DependencyInjection;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.IntegrationTests.Integration.Tests.Helpers
{

    public static class DiSetup

    {

        public static ServiceProvider Services;
        public static DebugLogger Logger;
        public static ILoggerCore LoggerCore;

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
                .AddTransient(typeof(ISettingsTestModel), typeof(SettingsTestModel))
                .AddTransient(typeof(DiLoggingTestClass));
        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))
                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment));
        }
    }
}
