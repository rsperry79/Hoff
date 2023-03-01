using Hoff.Core.Hardware.Storage.At24;
using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Services;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;

using NFUnitTest.Models;

namespace Hoff.Core.Common.Services.Settings.Tests.Helpers
{
    public static class TestHelpers

    {
        private static IEeprom prom;

        public static ILogger Logger;
        public static Settings<SettingsTestModel> Settings;
        public static ServiceProvider Services;

        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
            .AddSingleton(typeof(Settings<SettingsTestModel>))
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IEeprom), typeof(At24c256Eeprom))
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
                string loggerName = "TestLogger";

                // Setup
                LogLevel minLogLevel = LogLevel.Trace;
                Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);


                Services = ConfigureServices();

                IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();
                espConfig.SetI2C2Pins();
                prom = (IEeprom)Services.GetRequiredService(typeof(IEeprom));
                prom.DefaultInit();



                Settings = (Settings<SettingsTestModel>)Services.GetRequiredService(typeof(Settings<SettingsTestModel>));
            }

            return Settings;
        }
    }
}
