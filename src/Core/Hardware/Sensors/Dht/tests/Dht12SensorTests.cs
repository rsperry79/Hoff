using System.Threading;

using Hoff.Core.Interfaces;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.DHTxx;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

using Sensors.Environmental.Dht.Tests.Helpers;

namespace Sensors.Dht.Tests
{
    [TestClass]
    public class Dht11SensorTests
    {
        public static DebugLogger logger = null;

        public IDht11Sensor Setup()
        {
            ServiceProvider services = DiSetup.ConfigureServices();
            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            logger = loggerCore.GetDebugLogger("TestLogger", LogLevel.Trace);

            IDht11Sensor dht11Sensor = (IDht11Sensor)services.GetRequiredService(typeof(IDht11Sensor));
            dht11Sensor.Init(4);
            return dht11Sensor;
        }

        //[TestMethod]
        //public void CanTrackChangesTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = this.Setup();

        //    //// Act
        //    bool result = dht11Sensor.CanTrackChanges();

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]

        //public void HumidityTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = this.Setup();

        //    //// Act
        //    double result = dht11Sensor.Humidity;
        //    logger.LogDebug($"Humidity: {result}");

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void TemperatureTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = this.Setup();

        //    // Act
        //    double result = dht11Sensor.Temperature;
        //    logger.LogDebug($"Temperature: {result}");
        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void TrackChangesTest()
        //{
        //    // Arrange
        //    IDht11Sensor dht11Sensor = this.Setup();


        //    // Act
        //    dht11Sensor.BeginTrackChanges(50);
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    dht11Sensor.EndTrackChanges();
        //    // Assert
        //    Assert.IsNotNull(dht11Sensor);
        //}

        //[TestMethod]

        //        public void RawTest()
        //        {
        //            ServiceProvider services = DiSetup.ConfigureServices();
        //            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
        //            logger = loggerCore.GetDebugLogger("TestLogger", LogLevel.Trace);

        //            // Arrange
        //            Dht11 Dht = new Dht11(4);

        //            int index = 0;
        //            // Act
        //            do
        //            {
        //                UnitsNet.RelativeHumidity humidity = Dht.Humidity;
        //                UnitsNet.Temperature temp = Dht.Temperature;

        //                logger.LogDebug($"READ: {Dht.IsLastReadSuccessful}");
        //                Thread.Sleep(TimeSpan.FromSeconds(1));
        //                index++;

        //                if (Dht.IsLastReadSuccessful)
        //                {
        //                    logger.LogDebug($"Humidity: {humidity.Percent}");
        //                    logger.LogDebug($"Temperature: {temp.DegreesCelsius}");

        //                }
        //            }
        //            while (!Dht.IsLastReadSuccessful && index < 120);
        //}
        [TestMethod]

        public void RawTest2()
        {
            ServiceProvider services = DiSetup.ConfigureServices();
            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            logger = loggerCore.GetDebugLogger("TestLogger", LogLevel.Trace);

            using (Dht11 dht = new Dht11(26))
            {
                UnitsNet.Temperature temperature = dht.Temperature;
                UnitsNet.RelativeHumidity humidity = dht.Humidity;
                Thread.Sleep(2000);
                // You can only display temperature and humidity if the read is successful otherwise, this will raise an exception as
                // both temperature and humidity are NAN
                if (dht.IsLastReadSuccessful)
                {
                    logger.LogDebug($"Temperature: {temperature.DegreesCelsius} \u00B0C, Humidity: {humidity.Percent} %");

                }
                else
                {
                    logger.LogDebug("Error reading DHT sensor");
                }
            }
        }

        //[TestMethod]
        //public void DisposeTest()
        //{
        //    // Arrange
        //    IHumidityTemperatureSensor dht11Sensor = new dht11Sensor();

        //    // Act
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    dht11Sensor.Dispose();

        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    GC.ReRegisterForFinalize(dht11Sensor);
        //    // Assert
        //    Assert.IsNull(dht11Sensor);
        //}
    }
}
