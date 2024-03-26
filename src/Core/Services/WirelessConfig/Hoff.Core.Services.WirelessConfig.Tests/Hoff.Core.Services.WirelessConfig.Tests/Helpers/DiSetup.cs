using System;

using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.WirelessConfig;
using Hoff.Core.Services.WirelessConfig.Tests;
using Hoff.Services.Common.Interfaces.Storage;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoff.Core.Services.Settings.Tests.Helpers
{
    public static class DiSetup

    {
        public static ServiceProvider Services;
        private static ILoggerCore core;
        private static IServiceCollection serviceCollection;

        public static ServiceProvider ConfigureServices()
        {
            if (Services == null)
            {
                Console.WriteLine("Configuring Services");
                serviceCollection = new ServiceCollection();
                LoadSingleTons();
                LoadTransients();
                Services = serviceCollection.BuildServiceProvider();
            }

            return Services;
        }

        private static void LoadTransients()
        {
        }

        private static void LoadSingleTons()
        {
            serviceCollection = serviceCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))

                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))

                .AddSingleton(typeof(IWifiSettings), typeof(WifiDefaults))
                .AddSingleton(typeof(IApConfig), typeof(ApConfig))
                .AddSingleton(typeof(IWifiEnvironment), typeof(WifiEnvironment));

        }

        internal static ILogger ConfigureLogger(string name)
        {
            if (core == null)
            {
                Console.WriteLine("Configuring Logger");
                core = (ILoggerCore)Services.GetService(typeof(ILoggerCore));
                core.SetDefaultLoggingLevel(LogLevel.Trace);
            }

            return core.GetDebugLogger(name);
        }
    }
}
