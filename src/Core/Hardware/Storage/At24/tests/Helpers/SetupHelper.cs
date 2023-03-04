﻿using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Storage;
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
        public static IEeprom prom;
        public static ServiceProvider Services;
        public static DebugLogger Logger;
        private static int size = 32;
        public static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
             .AddSingleton(typeof(IEeprom), typeof(At24cEeprom))
             .BuildServiceProvider();

            return services;
        }

        public static IEeprom Setup()
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

                prom = (IEeprom)Services.GetRequiredService(typeof(IEeprom));
                prom.DefaultInit(size);
            }
            return prom;
        }
    }
}
