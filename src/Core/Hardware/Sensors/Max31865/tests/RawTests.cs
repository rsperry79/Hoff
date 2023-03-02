using System;
using System.Device.Spi;
using System.Threading;

using Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests.Helpers;

using Iot.Device.Max31865;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests
{
    [TestClass]
    public class RawTests
    {
        [TestMethod]
        public void RawTest()
        {
            SetupHelpers.BaseSetup();
            ILogger logger = SetupHelpers.Logger;

            const int busId = 1;
            const int selectPin = 15;
            try
            {
                SpiConnectionSettings settings = new(busId, selectPin)
                {
                    ClockFrequency = Max31865.SpiClockFrequency,
                    Mode = Max31865.SpiMode3,
                    DataFlow = Max31865.SpiDataFlow
                };

                using SpiDevice device = SpiDevice.Create(settings);
                using Max31865 sensor = new(device, PlatinumResistanceThermometerType.Pt100, ResistanceTemperatureDetectorWires.TwoWire, ElectricResistance.FromOhms(430));

                int index = 0;
                do
                {
                    // Print out the measured data
                    logger.LogDebug($"Temperature: {sensor.Temperature.DegreesFahrenheit}\u00B0F");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    index++;
                }
                while (index < 10);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
