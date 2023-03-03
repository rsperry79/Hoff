using Hoff.Core.Common.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Services;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Iot.Device.At24cxx;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Storage.At24.Tests.Helpers
{
    public static class SetupHelper
    {
        public static IEeprom<At24c256> prom;
        public static ServiceProvider Services;
        public static DebugLogger Logger;

        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
             .AddSingleton(typeof(IEeprom<At24c256>), typeof(At24cEeprom<>))
             .BuildServiceProvider();

            return services;
        }

        public static IEeprom<At24c256> Setup()
        {
            if (prom is null)
            {         // Arrange
                LoggerCore loggerCore = new();
                const string loggerName = "TestLogger";
                const LogLevel minLogLevel = LogLevel.Trace;
                Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

                Services = ConfigureServices();

                IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();
                espConfig.SetI2C2Pins();

                prom = (IEeprom<At24c256>)Services.GetRequiredService(typeof(IEeprom<At24c256>));
                prom.DefaultInit();
            }
            return prom;
        }
    }
}
