using System;
using System.Device.I2c;

using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;
using Hoff.Core.Hardware.Rtc.RtcDevice.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Rtc.RtcDevice.Tests
{
    [TestClass]
    public class DS3231RtcTests
    {
        [TestMethod]
        public void DefaultInitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();
            IDS3231Rtc dS3231Rtc = (IDS3231Rtc)SetupHelper.Services.GetRequiredService(typeof(IDS3231Rtc));

            // Act
            bool result = dS3231Rtc.DefaultInit();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();
            IDS3231Rtc dS3231Rtc = (IDS3231Rtc)SetupHelper.Services.GetRequiredService(typeof(IDS3231Rtc));

            const int bussId = 1;
            const byte deviceAddr = 0x68;
            const I2cBusSpeed busSpeed = I2cBusSpeed.FastMode;

            // Act
            bool result = dS3231Rtc.Init(bussId, deviceAddr, busSpeed);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReadDateTimeTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();

            // Act
            DateTime result = dS3231Rtc.ReadDateTime();

            // Assert
            Assert.AreEqual(typeof(DateTime), result.GetType());
        }

        [TestMethod]
        public void SetDateTimeTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();
            DateTime time = DateTime.UtcNow;

            // Act
            dS3231Rtc.SetDateTime(time);
        }

        [TestMethod]
        public void ReadTempTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();

            // Act
            Temperature result = dS3231Rtc.GetRtcTemperature();

            // Assert
            Assert.IsTrue(result.DegreesCelsius > 0);
        }
    }
}
