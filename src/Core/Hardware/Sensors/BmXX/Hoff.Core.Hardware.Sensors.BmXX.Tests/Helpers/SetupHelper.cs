using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers
{
    public static class SetupHelper
    {
        #region Fields

        public static DebugLogger Logger;
        public static ServiceProvider Services;

        private static IBme280Sensor sensor;

        #endregion Fields

        #region Public Methods

        public static void BaseSetup()
        {
            Services = ConfigureServices();

            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            ILoggerCore loggerCore = (ILoggerCore)Services.GetRequiredService(typeof(ILoggerCore));
            loggerCore.SetDefaultLoggingLevel(minLogLevel);

            Logger = loggerCore.GetDebugLogger(loggerName);

            IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();
        }

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
                _ = sensor.DefaultInit();
            }

            return sensor;
        }

        #endregion Public Methods
    }
}
