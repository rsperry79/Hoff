
using Hoff.Core.Interfaces;
using Hoff.Core.Sensors.HTU21D;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

using Sensors.Environmental.HTU21D.Tests.Helpers;

namespace Sensors.Environmental.Tests
{
    [TestClass]
    public class HTU21DTests
    {

        public static DebugLogger logger = null;

        public HTU21DSenor Setup()
        {
            SetCoreServices();

            HTU21DSenor hTU21D = new HTU21DSenor();
            hTU21D.Init();

            return hTU21D;
        }

        private static void SetCoreServices()
        {
            ServiceProvider services = DiSetup.ConfigureServices();
            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            logger = loggerCore.GetDebugLogger("TestLogger", LogLevel.Trace);

            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();
        }

        [TestMethod]
        public void Raw()
        {
            HTU21DSenor hTU21D = this.Setup();    
        }

        //[TestMethod]
        //public void CanTrackChangesTest()
        //{

        //    // Arrange
        //    HTU21DSenor hTU21D = this.Setup();

        //    // Act
        //    bool result = hTU21D.CanTrackChanges();

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void HumidityTest()
        //{
        //    // Arrange
        //    HTU21DSenor hTU21D = this.Setup();
        //    // Act
        //    double result = hTU21D.Humidity;

        //    logger.LogDebug($"Humidity: {result}");
        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void TemperatureTest()
        //{
        //    // Arrange
        //    HTU21DSenor hTU21D = this.Setup();
        //    // Act
        //    double result = hTU21D.Temperature;

        //    logger.LogDebug($"Temp: {result}");

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result);
        //}


        // [TestMethod]
        //public void DisposeTest()
        //{
        //    // Arrange
        //    IHumidityTemperatureSensor hTU21D = new HTU21D();

        //    // Act
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    hTU21D.Dispose();
        //    // Assert
        //    Assert.IsNull(hTU21D);
        // }
    }
}
