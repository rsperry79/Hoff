using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Sensors.Dht.Interfaces;
using Hoff.Core.Services.Logging;
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
        #region Fields

        internal static DebugLogger Logger;
        internal static int Pin = 11;

        internal static ServiceProvider Services;

        private static IDht11Sensor sensor;

        #endregion Fields

        #region Public Methods

        public static void BaseSetup()
        {
            Services = ConfigureServices();

            LoggerCore loggerCore = new();
            const string loggerName = "TestLogger";

            // Setup
            const LogLevel minLogLevel = LogLevel.Trace;
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
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

        #endregion Public Methods

        #region Internal Methods

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

        #endregion Internal Methods
    }
}
