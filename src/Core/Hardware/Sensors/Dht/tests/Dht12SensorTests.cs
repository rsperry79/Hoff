using System;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Sensors.Dht.Tests.Helpers;

using Iot.Device.DHTxx;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.Dht.Tests
{
    [TestClass]
    public class Dht12SensorTests

    {
        //[TestMethod]
        //public void CanTrackChangesTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = SetupHelper.Setup();

        //    //// Act
        //    bool result = dht11Sensor.CanTrackChanges();

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]

        //public void HumidityTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = SetupHelper.Setup();

        //    //// Act
        //    RelativeHumidity result = dht11Sensor.Humidity;
        //    SetupHelper.Logger.LogDebug($"Humidity: {result}");

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void TemperatureTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = SetupHelper.Setup();

        //    // Act
        //    Temperature result = dht11Sensor.Temperature;
        //    SetupHelper.Logger.LogDebug($"Temperature: {result}");

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void TrackChangesTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = SetupHelper.Setup();

        //    // Act
        //    dht11Sensor.BeginTrackChanges(50);
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    dht11Sensor.EndTrackChanges();

        //    // Assert
        //    Assert.IsNotNull(dht11Sensor);
        //}

        #region Public Methods

        [TestMethod]
        public void RawTest()
        {
            ServiceProvider services = SetupHelper.ConfigureServices();
            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            nanoFramework.Logging.Debug.DebugLogger logger = loggerCore.GetDebugLogger("TestLogger", LogLevel.Trace);

            // Arrange
            Dht11 Dht = new Dht11(6);

            int index = 0;
            // Act

            RelativeHumidity humidity = Dht.Humidity;
            Temperature temp = Dht.Temperature;
            do
            {
                logger.LogDebug($"READ: {Dht.IsLastReadSuccessful}");
                Thread.Sleep(TimeSpan.FromSeconds(1));
                index++;

                if (Dht.IsLastReadSuccessful)
                {
                    logger.LogDebug($"Humidity: {humidity.Percent}");
                    logger.LogDebug($"Temperature: {temp.DegreesCelsius}");
                }
            }
            while (!Dht.IsLastReadSuccessful && index < 120);
        }

        #endregion Public Methods

        //[TestMethod]
        //public void RawTest2()
        //{
        //    using (IDht11Sensor dht = SetupHelper.Setup())
        //    {
        //        UnitsNet.Temperature temperature = dht.Temperature;
        //        UnitsNet.RelativeHumidity humidity = dht.Humidity;
        //        Thread.Sleep(2000);
        //        // You can only display temperature and humidity if the read is successful otherwise, this will raise an exception as
        //        // both temperature and humidity are NAN

        //        SetupHelper.Logger.LogDebug($"Temperature: {temperature.DegreesCelsius} \u00B0C, Humidity: {humidity.Percent} %");
        //    }
        //}

        //[TestMethod]
        //public void DisposeTest()
        //{
        //    // Arrange
        //    using (IDht11Sensor dht11Sensor = SetupHelper.Setup())
        //    {
        //    }
        //}
    }
}
