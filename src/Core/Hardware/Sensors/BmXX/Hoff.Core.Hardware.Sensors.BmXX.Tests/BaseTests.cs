using System.Device.I2c;

using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;

using Microsoft.Extensions.DependencyInjection;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class BaseTests
    {
        #region Public Methods

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

        [TestMethod]
        public void DefaultInitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();

            IBme280Sensor bme280Sensor = (IBme280Sensor)SetupHelper.Services.GetRequiredService(typeof(IBme280Sensor));
            // Act
            bool result = bme280Sensor.DefaultInit();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();
            IBme280Sensor bme280Sensor = (IBme280Sensor)SetupHelper.Services.GetRequiredService(typeof(IBme280Sensor));

            int busSelector = 1;
            byte deviceAddr = 0x76;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;
            uint scale = 2;

            // Act
            _ = bme280Sensor.Init(
                busSelector,
                deviceAddr,
                speed,
                scale);

            // Assert
            Assert.IsNotNull(bme280Sensor);
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

        #endregion Public Methods
    }
}
