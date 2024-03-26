using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Rtc.RtcDevice.Tests.Helpers
{
    public static class SetupHelper
    {
        #region Fields

        public static DebugLogger Logger;
        public static ServiceProvider Services;

        private static IDS3231Rtc sensor;

        #endregion Fields

        #region Public Methods

        public static void BaseSetup()
        {
            Logger = LoadLogging();

            try
            {
                Services = ConfigureServices();
                LoadEsp32I2c();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                throw;
            }
        }

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

        public static void LoadEsp32I2c()
        {
            try
            {
                IEspConfig espConfig = (IEspConfig)Services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();
                espConfig.SetI2C2Pins();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public static DebugLogger LoadLogging()
        {
            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;

            LoggerCore loggerCore = new(); // (ILoggerCore)Services.GetRequiredService(typeof(ILoggerCore));
            loggerCore.SetDefaultLoggingLevel(minLogLevel);

            return loggerCore.GetDebugLogger(loggerName);
        }

        public static IDS3231Rtc Setup()
        {
            if (sensor is null)
            {
                BaseSetup();

                try
                {
                    sensor = (IDS3231Rtc)Services.GetRequiredService(typeof(IDS3231Rtc));
                    _ = sensor.DefaultInit();
                }
                catch (System.Exception ex)
                {
                    Logger.LogError(ex.StackTrace);
                    throw;
                }
            }

            return sensor;
        }

        #endregion Public Methods
    }
}
