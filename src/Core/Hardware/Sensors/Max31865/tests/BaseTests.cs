
using System.Device.Spi;

using Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces;
using Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests.Helpers;

using Iot.Device.Max31865;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests
{
    [TestClass]
    public class BaseTests
    {

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            int bussId = 1;
            int selectPin = 17;
            SpiMode spiMode = SpiMode.Mode3;
            PlatinumResistanceThermometerType thermometerType = PlatinumResistanceThermometerType.Pt100;
            ResistanceTemperatureDetectorWires wires = ResistanceTemperatureDetectorWires.TwoWire;
            int resistance = 4300;
            uint scale = 2;

            SetupHelpers.BaseSetup();
            IMax31865Senor maxSensor = (IMax31865Senor)SetupHelpers.Services.GetRequiredService(typeof(IMax31865Senor));


            // Act
            maxSensor.Init(bussId, selectPin, spiMode, thermometerType, wires, resistance, scale);

            // Assert
            Assert.IsNotNull(maxSensor);
        }


        [TestMethod]
        public void DefaultInitTest()
        {
            // Arrange
            SetupHelpers.BaseSetup();
            IMax31865Senor maxSensor = (IMax31865Senor)SetupHelpers.Services.GetRequiredService(typeof(IMax31865Senor));

            // Act
            bool result = maxSensor.DefaultInit();

            // Assert
            Assert.IsTrue(result);
        }




        [TestMethod]
        public void CanTrackChangesTest()
        {
            // Arrange
            IMax31865Senor bme280Sensor = SetupHelpers.Setup();

            // Act
            bool result = bme280Sensor.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
