
using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Sensors.Environmental;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using Sensors.Environmental.Tests.Helpers;

namespace Sensors.Environmental.Tests
{
    [TestClass]
    public class HTU21DTests
    {
        [Setup]
        public void Setup()
        {
            DiSetup s = new DiSetup();
            ServiceProvider services = s.ConfigureServices();

            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();

        }

        [TestMethod]
        public void CanTrackChangesTest()
        {


            // Arrange
            IHumidityTempatureSensor hTU21D = new HTU21D();

            // Act
            bool result = hTU21D.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HumidityTest()
        {
            // Arrange
            IHumidityTempatureSensor htu21d = new HTU21D();

            // Act
            double result = htu21d.Humidity;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TempatureTest()
        {
            // Arrange
            IHumidityTempatureSensor htu21d = new HTU21D();

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
        //    IHumidityTempatureSensor hTU21D = new HTU21D();

        //    // Act
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    hTU21D.Dispose();
        //    // Assert
        //    Assert.IsNull(hTU21D);
        // }
    }
}
