

using Hoff.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Sensors.Max31865Senor;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;


namespace Sensors.Environmental.Max31865Sensor.Tests.Helpers
{
    [TestClass]
    public class Max31865SenorTests
    {
        [Setup]
        public void Setup()
        {
            DiSetup s = new();
            ServiceProvider services = s.ConfigureServices();

            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetSpi1Pins();

        }

        [TestMethod]

        public void CanTrackChangesTest()
        {
            // Arrange
            ITempatureSensor max31865Senor = new Max31865Senor();

            // Act
            bool result = max31865Senor.CanTrackChanges();

            // Assert
            Assert.IsTrue(result);
        }

        //[TestMethod]
        //public void DisposeTest()
        //{
        //    // Arrange
        //    ITempatureSensor max31865Senor = new Max31865Senor();

        //    // Act
        //    Thread.Sleep(TimeSpan.FromSeconds(1));
        //    max31865Senor.Dispose();

        //    // Assert
        //    Assert.IsNull(max31865Senor);
        //}

        [TestMethod]
        public void TempatureTest()
        {
            // Arrange
            ITempatureSensor max31865Senor = new Max31865Senor();

            // Act
            double result = max31865Senor.Temperature;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
        }

    }
}
