using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

using System;
using System.Device.I2c;
using System.Threading;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class RawTests
    {
        [TestMethod]
        public void RawTest()
        {
            SetupHelper.BaseSetup();
            ILogger logger = SetupHelper.Logger;
            const int busId = 1;

            try
            {
                I2cDevice i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, 0x76, I2cBusSpeed.FastMode));
                Bme280 i2CBmp280 = new Bme280(i2cDevice);

                // set higher sampling
                i2CBmp280.TemperatureSampling = Sampling.UltraHighResolution;
                i2CBmp280.PressureSampling = Sampling.UltraHighResolution;
                i2CBmp280.HumiditySampling = Sampling.UltraHighResolution;


                // Perform a synchronous measurement
                do
                {
                    Bme280ReadResult readResult;

                    do
                    {
                        readResult = i2CBmp280.Read();
                        Thread.Sleep(100);
                    }
                    while (!readResult.TemperatureIsValid);

                    i2CBmp280.TryReadAltitude(out UnitsNet.Length altValue);

                    // Print out the measured data
                    logger.LogDebug($"Humidity: {readResult.Humidity.Percent}%");
                    logger.LogDebug($"Temperature: {readResult.Temperature.DegreesFahrenheit}\u00B0F");
                    logger.LogDebug($"Pressure: {readResult.Pressure.InchesOfMercury}inHg");
                    logger.LogDebug($"Altitude: {altValue.Feet}ft");

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                while (true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
