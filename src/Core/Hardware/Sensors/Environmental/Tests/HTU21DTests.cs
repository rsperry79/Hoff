﻿//using System.Diagnostics;

//using Hoff.Hardware.Common.Interfaces;
//using Hoff.Hardware.Common.Interfaces.Services;

//using nanoFramework.DependencyInjection;
//using nanoFramework.TestFramework;

//namespace Hoff.Hardware.Sensors.Environmental.Tests
//{
//    [TestClass]
//    public class HTU21DTests
//    {
//        [Setup]
//        public void Setup()
//        {

//            ServiceProvider services = DiSetup.ConfigureServices();
//            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
//            Debug.WriteLine("pre");

//            espConfig.SetI2C1Pins();
//            espConfig.SetSpi1Pins();
//            Debug.WriteLine("ExitSetup");
//        }

//        [TestMethod]
//        public void CanTrackChangesTest()
//        {


//            // Arrange
//            IHumidityTempatureSensor hTU21D = new HTU21D();

//            // Act
//            bool result = hTU21D.CanTrackChanges();

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        public void HumidityTest()
//        {
//            // Arrange
//            IHumidityTempatureSensor hTU21D = new HTU21D();

//            // Act
//            UnitsNet.RelativeHumidity result = hTU21D.Humidity;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Percent);
//        }

//        [TestMethod]
//        public void TempatureTest()
//        {
//            // Arrange
//            IHumidityTempatureSensor hTU21D = new HTU21D();

//            // Act
//            UnitsNet.Temperature result = hTU21D.Temperature;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.DegreesCelsius);
//        }


//        // [TestMethod]
//        //public void DisposeTest()
//        //{
//        //    // Arrange
//        //    IHumidityTempatureSensor hTU21D = new HTU21D();

//        //    // Act
//        //    Thread.Sleep(TimeSpan.FromSeconds(1));
//        //    hTU21D.Dispose();
//        //    // Assert
//        //    Assert.IsNull(hTU21D);
//        // }
//    }
//}