

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Services;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Rtc.RtcDevice.Tests.Helpers
{
    public static class SetupHelper
    {
        private static IDS3231Rtc sensor;
        public static ServiceProvider Services;
        public static DebugLogger Logger;

        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .AddSingleton(typeof(IDS3231Rtc), typeof(DS3231Rtc))
             .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
             .BuildServiceProvider();

            return services;
        }

        public static IDS3231Rtc Setup()
        {
            if (sensor is null)
            {
                BaseSetup();

                sensor = (IDS3231Rtc)Services.GetRequiredService(typeof(IDS3231Rtc));
                _ = sensor.DefaultInit();
            }

            return sensor;
        }

        public static void BaseSetup()
        {
            Services = ConfigureServices();

            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            ILoggerCore loggerCore = (ILoggerCore)Services.GetRequiredService(typeof(ILoggerCore));
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

            IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();
        }
    }
}
