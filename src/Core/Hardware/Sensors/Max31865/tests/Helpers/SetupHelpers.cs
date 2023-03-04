using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests.Helpers
{
    internal static class SetupHelpers
    {
        public static ServiceProvider Services;
        public static DebugLogger Logger;
        private static IMax31865Senor sensor;

        internal static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
                 .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                 .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                 .AddSingleton(typeof(IMax31865Senor), typeof(Max31865Senor))
                 .AddSingleton(typeof(IEspConfig), typeof(EspConfig))

             .BuildServiceProvider();

            return services;
        }
        public static IMax31865Senor Setup()
        {
            if (sensor is null)
            {
                BaseSetup();

                sensor = (IMax31865Senor)Services.GetRequiredService(typeof(IMax31865Senor));
                sensor.DefaultInit();
            }

            return sensor;
        }

        public static void BaseSetup()
        {
            Services = ConfigureServices();

            LoggerCore loggerCore = new();
            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

            IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetSpi1Pins();
        }
    }
}
