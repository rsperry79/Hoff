using System;
using System.Device.I2c;

using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;
using Hoff.Core.Hardware.Rtc.RtcDevice.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Rtc.RtcDevice.Tests
{
    [TestClass]
    public class DS3231RtcTests
    {
        //public void RawTest()
        //{
        //    const string loggerName = "TestLogger";
        //    const LogLevel minLogLevel = LogLevel.Trace;

        //    LoggerCore loggerCore = new LoggerCore(); // (ILoggerCore)Services.GetRequiredService(typeof(ILoggerCore));
        //    ILogger logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
        //}

        #region Public Methods

        [TestMethod]
        public void DefaultInitTest()
        {
            // Arrange
            ILogger logger = SetupHelper.LoadLogging();
            SetupHelper.BaseSetup();
            try
            {
                DS3231Rtc dS3231Rtc = new DS3231Rtc(new I2cBussControllerService());

                // Act
                bool result = dS3231Rtc.DefaultInit();

                // Assert
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.BaseSetup();
            ILogger logger = SetupHelper.Logger;
            try
            {
                IDS3231Rtc dS3231Rtc = (IDS3231Rtc)SetupHelper.Services.GetRequiredService(typeof(IDS3231Rtc));

                const int bussId = 1;
                const byte deviceAddr = 0x68;
                const I2cBusSpeed busSpeed = I2cBusSpeed.FastMode;

                // Act
                bool result = dS3231Rtc.Init(bussId, deviceAddr, busSpeed);

                // Assert
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void ReadDateTimeTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();

            // Act
            DateTime result = dS3231Rtc.ReadDateTime();
            ILogger logger = SetupHelper.Logger;
            logger.LogDebug($"Read: {result}");
            // Assert
            Assert.AreEqual(typeof(DateTime), result.GetType());
        }

        [TestMethod]
        public void ReadTempTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();
            ILogger logger = SetupHelper.Logger;
            // Act
            Temperature result = dS3231Rtc.GetRtcTemperature();
            logger.LogDebug($"Read: {result.DegreesCelsius}C");

            // Assert
            Assert.IsTrue(result.DegreesCelsius > 0);
        }

        [TestMethod]
        public void SetDateTimeTest()
        {
            // Arrange
            IDS3231Rtc dS3231Rtc = SetupHelper.Setup();
            ILogger logger = SetupHelper.Logger;

            // Act
            DateTime date = new DateTime(2023, 3, 3, 10, 11, 12);

            logger.LogDebug($"Set: {date}");
            dS3231Rtc.SetDateTime(date);

            DateTime result = dS3231Rtc.ReadDateTime();
            logger.LogDebug($"Read: {result}");
        }

        #endregion Public Methods
    }
}
