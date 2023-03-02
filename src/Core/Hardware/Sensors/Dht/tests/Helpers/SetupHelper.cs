using Hoff.Core.Hardware.Sensors.Dht.Interfaces;
using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Sensors.Dht.Tests.Helpers
{
    internal static class SetupHelper
    {
        internal static int Pin = 11;

        private static IDht11Sensor sensor;
        internal static ServiceProvider Services;
        internal static DebugLogger Logger;

        internal static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IDht11Sensor), typeof(Dht11Sensor))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .BuildServiceProvider();

            return services;
        }

        public static IDht11Sensor Setup()
        {
            if (sensor is null)
            {
                BaseSetup();
                sensor = (IDht11Sensor)Services.GetRequiredService(typeof(IDht11Sensor));
                sensor.Init(Pin);
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

        }
    }
}
