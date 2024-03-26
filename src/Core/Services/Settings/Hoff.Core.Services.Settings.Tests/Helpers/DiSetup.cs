using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.WirelessConfig.Models;
using Hoff.Services.Common.Interfaces.Storage;
using Hoff.Tests.Common.Interfaces;
using Hoff.Tests.Common.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Services.Settings.Tests.Helpers
{
    public static class DiSetup

    {

        public static ServiceProvider Services;
        public static DebugLogger Logger;
        public static ILoggerCore LoggerCore;

        private static readonly ServiceCollection serviceCollection = new();

        public static DebugLogger GetLogger()
        {
            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;

            LoggerCore = new LoggerCore();
            Logger = LoggerCore.GetDebugLogger(loggerName);

            // Setup
            LoggerCore.SetDefaultLoggingLevel(minLogLevel);
            Logger = LoggerCore.GetDebugLogger(loggerName);
            return Logger;
        }

        public static ServiceProvider ConfigureServices()
        {
            LoadSingleTons();
            LoadTransients();
            Services = serviceCollection.BuildServiceProvider();
            return Services;
        }

        private static void LoadTransients()
        {
            _ = serviceCollection.AddTransient(typeof(ISettingsTestModel), typeof(SettingsTestModel));

        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings));
        }
    }
}
