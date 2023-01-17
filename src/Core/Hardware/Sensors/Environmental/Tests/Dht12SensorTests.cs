using System;
using System.Threading;

using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Services;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Sensors.Environmental.Tests
{
    [TestClass]
    public class Dht12SensorTests
    {
        [Setup]
        public void Setup()
        {

            ServiceProvider services = DiSetup.ConfigureServices();
            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
        }

        [TestMethod]
        public void CanTrackChangesTest()
        {
            // Arrange
            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

            // Act
            bool result = dht12Sensor.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HumidityTest()
        {
            // Arrange
            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

            // Act
            UnitsNet.RelativeHumidity result = dht12Sensor.Humidity;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Percent);
        }

        [TestMethod]
        public void TempatureTest()
        {
            // Arrange
            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

            // Act
            UnitsNet.Temperature result = dht12Sensor.Temperature;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DegreesCelsius);
        }

        [TestMethod]
        public void TrackChangesTest()
        {
            // Arrange
            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

            // Act
            dht12Sensor.BeginTrackChanges(50);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            dht12Sensor.EndTrackChanges();
            // Assert
            Assert.IsNotNull(dht12Sensor);
        }

        [TestMethod]
        public void DisposeTest()
        {
            // Arrange
            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

            // Act
            Thread.Sleep(TimeSpan.FromSeconds(1));
            dht12Sensor.Dispose();
            // Assert
            Assert.IsNull(dht12Sensor);
        }
    }
}
