using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.Bmxx80;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

using System;
using System.Device.I2c;
using System.Threading;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class Bme280SensorTests
    {
        
[TestMethod]
        public void HumidityTest()
        {
            // Arrange

            IBme280Sensor bme280Sensor = SetupHelper.Setup();

            double x = bme280Sensor.Humidity;


        }
        //[TestMethod]
        //public void InitTest()
        //{
        //    // Arrange
        //    IBme280Sensor bme280Sensor = SetupHelper.Setup();


        //    int busSelector = 0;
        //    byte deviceAddr = 0;
        //    I2cBusSpeed speed = default(global::System.Device.I2c.I2cBusSpeed);
        //    uint scale = 0;

        //    // Act
        //    bme280Sensor.Init(
        //        busSelector,
        //        deviceAddr,
        //        speed,
        //        scale);

        //    // Assert
        //}

        //[TestMethod]
        //public void DefaultInitTest()
        //{
        //    // Arrange
        //    IBme280Sensor bme280Sensor = SetupHelper.Setup();

        //    // Act
        //    var result = bme280Sensor.DefaultInit();

        //    // Assert
        //}

        //[TestMethod]
        //public void InitTest1()
        //{
        //    // Arrange
        //    IBme280Sensor bme280Sensor = SetupHelper.Setup();
        //    int bussId = 0;
        //    uint scale = 2;
        //    byte deviceAddr = 0;
        //    I2cBusSpeed busSpeed = default(global::System.Device.I2c.I2cBusSpeed);

        //    // Act
        //    var result = bme280Sensor.Init(
        //        bussId,
        //        deviceAddr,
        //        busSpeed, scale);

        //    // Assert
        //}

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
            var result = bme280Sensor.CanTrackChanges();

            // Assert
        }

        //[TestMethod]
        //public void HasSensorValueChangedTest()
        //{
        //    // Arrange
        //    IBme280Sensor bme280Sensor = SetupHelper.Setup();

        //    // Act
        //    bme280Sensor.HasSensorValueChanged();

        //    // Assert
        //}

        //[TestMethod]
        public void DisposeTest()
        {
            // Arrange

            using (IBme280Sensor bme280Sensor = SetupHelper.Setup())
            { }

   
        }
    }
}
