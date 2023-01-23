
//using Hoff.Hardware.Common.Interfaces;
//using Hoff.Hardware.Common.Interfaces.Config;
//using Hoff.Hardware.Common.Interfaces.Services;
//using Hoff.Hardware.Common.Models;
//using Hoff.Hardware.Sensors.Environmental;
//using Hoff.Hardware.SoC.SoCEsp32;

//using nanoFramework.DependencyInjection;
//using nanoFramework.TestFramework;

//using Sensors.Environmental.Tests.Helpers;

//using System;
//using System.Threading;

//namespace Sensors.Environmental.Tests
//{
//    [TestClass]
//    public class Dht12SensorTests
//    {
//        [Setup]
//        public void Setup()
//        {
//            DiSetup s = new DiSetup();
//            ServiceProvider services = s.ConfigureServices();

//            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
//            espConfig.SetI2C1Pins();
//        }

//        [TestMethod]
//        public void CanTrackChangesTest()
//        {

//            // Arrange
//            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

//            //// Act
//            bool result = dht12Sensor.CanTrackChanges();

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        public void HumidityTest()
//        {
//            // Arrange
//            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

//            // Act
//            double result = dht12Sensor.Humidity;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TempatureTest()
//        {
//            // Arrange
//            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

//            // Act
//            double result = dht12Sensor.Temperature;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TrackChangesTest()
//        {
//            // Arrange
//            IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

//            // Act
//            dht12Sensor.BeginTrackChanges(50);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            dht12Sensor.EndTrackChanges();
//            // Assert
//            Assert.IsNotNull(dht12Sensor);
//        }

//        //[TestMethod]
//        //public void DisposeTest()
//        //{
//        //    // Arrange
//        //    IHumidityTempatureSensor dht12Sensor = new Dht12Sensor();

//        //    // Act
//        //    Thread.Sleep(TimeSpan.FromSeconds(1));
//        //    dht12Sensor.Dispose();

//        //    Thread.Sleep(TimeSpan.FromSeconds(1));
//        //    GC.ReRegisterForFinalize(dht12Sensor);
//        //    // Assert
//        //    Assert.IsNull(dht12Sensor);
//        //}
//    }
//}
