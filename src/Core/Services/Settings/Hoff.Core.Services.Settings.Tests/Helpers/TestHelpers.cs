using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Hardware.Storage.At24;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings.Tests.Models;

using Hoff.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;

namespace Hoff.Core.Services.Settings.Tests.Helpers
{
    public static class TestHelpers

    {
        #region Fields

        public static ILogger Logger;
        public static ServiceProvider Services;
        public static Settings<SettingsTestModel> Settings;

        private static IEeprom prom;
        private static readonly int size = 256;

        #endregion Fields

        #region Public Methods

        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
            .AddSingleton(typeof(Settings<SettingsTestModel>))
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IEeprom), typeof(At24cEeprom))
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
            .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
            .AddSingleton(typeof(Settings<SettingsTestModel>))
            .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .BuildServiceProvider();

            return services;
        }

        public static Settings<SettingsTestModel> Setup()
        {
            if (Settings is null)
            {         // Arrange
                LoggerCore loggerCore = new();
                const string loggerName = "TestLogger";

                // Setup
                const LogLevel minLogLevel = LogLevel.Trace;
                Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
                Services = ConfigureServices();

                IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();
                espConfig.SetI2C2Pins();
                prom = (IEeprom)Services.GetRequiredService(typeof(IEeprom));


                Settings = (Settings<SettingsTestModel>)Services.GetRequiredService(typeof(Settings<SettingsTestModel>));
            }

            return Settings;
        }

        #endregion Public Methods
    }
}
