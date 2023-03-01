using System.Device.I2c;

using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Services;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class BaseTests
    {

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();


            int busSelector = 1;
            byte deviceAddr = 0x76;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;
            uint scale = 2;

            II2cBussControllerService i2CBuss = (II2cBussControllerService)SetupHelper.Services.GetRequiredService(typeof(II2cBussControllerService));
            Bme280Sensor bme280Sensor = new Bme280Sensor(i2CBuss);

            // Act
            bme280Sensor.Init(
                busSelector,
                deviceAddr,
                speed,
                scale);

            // Assert

            Assert.IsNotNull(bme280Sensor);
        }

        [TestMethod]
        public void DefaultInitTest()
        {
            // Arrange
            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            bool result = bme280Sensor.DefaultInit();

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void ResetTest()
        {
            // Arrange
            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            bme280Sensor.Reset();

            // Assert
        }

        [TestMethod]
        public void CanTrackChangesTest()
        {
            // Arrange
            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            // Act
            bool result = bme280Sensor.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
