using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.WirelessConfig.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Core.Services.Settings.Tests.Helpers
{
    public static class DiSetup

    {

        public static ServiceProvider Services;

        private static readonly ServiceCollection serviceCollection = new();

        public static ServiceProvider ConfigureServices()
        {
            LoadSingleTons();
            LoadTransients();
            Services = serviceCollection.BuildServiceProvider();
            return Services;
        }

        private static void LoadTransients()
        {
            _ = serviceCollection
                .AddTransient(typeof(ISettingsStorageItem), typeof(SettingsStorageItem));
        }

        private static void LoadSingleTons()
        {
            _ = serviceCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(ISettingsStorage), typeof(SettingsStorage))
                .AddSingleton(typeof(IWifiSettings), typeof(WifiSettings))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService));
        }
    }
}
