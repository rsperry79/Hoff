using System;
using System.Threading;


using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Sensors.Environmental;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using Sensors.Environmental.Tests.Helpers;

namespace Sensors.Environmental.Tests
{
    [TestClass]
    public class Max31865SenorTests
    {
        [Setup]
        public void Setup()
        {
            DiSetup s = new DiSetup();
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

        [TestMethod]
        public void DisposeTest()
        {
            // Arrange
            ITempatureSensor max31865Senor = new Max31865Senor();

            // Act
            Thread.Sleep(TimeSpan.FromSeconds(1));
            max31865Senor.Dispose();

            // Assert
            Assert.IsNull(max31865Senor);
        }

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
