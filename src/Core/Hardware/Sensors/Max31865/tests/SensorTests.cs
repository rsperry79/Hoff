using Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces;
using Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests.Helpers;

using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests
{
    [TestClass]
    public class SensorTests
    {
        [TestMethod]
        public void TemperatureTest()
        {
            // Arrange
            IMax31865Senor bme280Sensor = SetupHelpers.Setup();

            // Act
            Temperature result = bme280Sensor.Temperature;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
