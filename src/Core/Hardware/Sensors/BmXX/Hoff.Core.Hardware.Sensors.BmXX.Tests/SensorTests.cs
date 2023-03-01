using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;

using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class SensorTests
    {

        [TestMethod]
        public void HumidityTest()
        {
            // Arrange

            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            RelativeHumidity result = bme280Sensor.Humidity;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TemperatureTest()
        {
            // Arrange

            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            Temperature result = bme280Sensor.Temperature;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PressureTest()
        {
            // Arrange

            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            Pressure result = bme280Sensor.Pressure;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AltitudeTest()
        {
            // Arrange

            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            Length result = bme280Sensor.Altitude;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
