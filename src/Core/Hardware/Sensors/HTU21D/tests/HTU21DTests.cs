
using Hoff.Core.Sensors.HTU21D;
using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using Sensors.Environmental.HTU21D.Tests.Helpers;

namespace Sensors.Environmental.Tests
{
    [TestClass]
    public class HTU21DTests
    {
        [Setup]
        public void Setup()
        {
            DiSetup s = new();
            ServiceProvider services = s.ConfigureServices();

            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();
        }

        [TestMethod]
        public void CanTrackChangesTest()
        {

            // Arrange
            IHumidityTempatureSensor hTU21D = new HTU21DSenor();

            // Act
            bool result = hTU21D.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HumidityTest()
        {
            // Arrange
            IHumidityTempatureSensor htu21d = new HTU21DSenor();

            // Act
            double result = htu21d.Humidity;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TemperatureTest()
        {
            // Arrange
            IHumidityTempatureSensor htu21d = new HTU21DSenor();

            // Act
            double result = htu21d.Temperature;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
        }


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
