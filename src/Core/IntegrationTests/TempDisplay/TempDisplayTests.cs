using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Hardware.Displays.Ssd13;
using Hoff.Hardware.Displays.Ssd13.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;
using Hoff.Core.Hardware.Sensors.BmXX;
using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;

using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using System.Collections;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Services;
using System;
using Hoff.Hardware.Common.Helpers;
using System.Threading;

namespace Hoff.Core.IntegrationTests.DependencyInjection.Tests.TempDisplay
{
    [TestClass]
    public class TempDisplayTests
    {

        public static Display Display;
        public static Bme280Sensor Sensor;
        public static DebugLogger Logger;
        [TestMethod]
        public void TempDisplayTest()
        {
            ServiceProvider  services = this.ConfigureServices();

            string loggerName = "TestLogger";
            LogLevel minLogLevel = LogLevel.Trace;
            LoggerCore loggerCore = new LoggerCore();
            Logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
            try
            {
                IEspConfig espConfig = new EspConfig(new PinConfig());

                espConfig.SetI2C1Pins();
                espConfig.SetI2C2Pins();
                I2cBussControllerService scanner = new I2cBussControllerService();

                Display = new Display(scanner);
                _ = Display.DefaultInit();

                Sensor = new Bme280Sensor(scanner);
                _ = Sensor.DefaultInit();
                Sensor.TemperatureSensorChanged += this.Sensor_TemperatureSensorChanged;    ;

                Display.WriteLine(10, 10, $"Temp: {Sensor.Temperature.DegreesFahrenheit.Truncate(2)}\u00B0F");
                Display.UpdateDisplay();
                Sensor.BeginTrackChanges(50);



                do
                {
                    Thread.Sleep(100);
                }
                while (true);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                throw;
            }
        }

        private bool Sensor_TemperatureSensorChanged()
        {
            Logger.LogDebug($"Changed Event");
                Display.WriteLine(10, 10, $"Temp: {Sensor.Temperature.DegreesFahrenheit:2}F");
                Display.UpdateDisplay();
            return true;
        }

        //private bool Sensor_TemperatureSensorChanged()
        //{

        //}

        public  ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
                .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
                .AddSingleton(typeof(ISsd13), typeof(Display))
                .AddSingleton(typeof(IBme280Sensor), typeof(Bme280Sensor))
                .BuildServiceProvider();
        }

        public void Setup()
        {
           


                


             
        }
    }
}
