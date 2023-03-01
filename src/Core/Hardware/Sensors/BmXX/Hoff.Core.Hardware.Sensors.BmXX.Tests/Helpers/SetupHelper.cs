using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Interfaces;
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

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers
{
    public static class SetupHelper
    {
        private static IBme280Sensor sensor;
        public static ServiceProvider Services;
        public static DebugLogger Logger;

        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .AddSingleton(typeof(IBme280Sensor), typeof(Bme280Sensor))
             .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
             .BuildServiceProvider();

            return services;
        }

        public static IBme280Sensor Setup()
        {
            if (sensor is null)
            {
                BaseSetup();

                sensor = (IBme280Sensor)Services.GetRequiredService(typeof(IBme280Sensor));
                sensor.DefaultInit();
            }

            return sensor;
        }

        public static void BaseSetup()
        {
            Services = ConfigureServices();

            LoggerCore loggerCore = new();
            string loggerName = "TestLogger";

            // Setup
            LogLevel minLogLevel = LogLevel.Trace;
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

            IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();
        }
    }
}
