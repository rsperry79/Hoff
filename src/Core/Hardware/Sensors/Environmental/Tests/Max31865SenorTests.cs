using Hoff.Hardware.Common.Interfaces;
using nanoFramework.TestFramework;
using System;
using System.Threading;

namespace Hoff.Hardware.Sensors.Environmental.Tests
{
    [TestClass]
    public class Max31865SenorTests
    {
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
            UnitsNet.Temperature result = max31865Senor.Temperature;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DegreesCelsius);
        }

    }
}
