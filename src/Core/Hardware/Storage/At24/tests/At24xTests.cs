using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using System;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class At24xTests
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
        public void TestMethod1()
        {
        }


        [TestMethod]
        public void WriteByteTest()
        {
            // Arrange
            IEeprom at24cEeprom = new At24cEeprom();
            byte address = 0;
            byte message = 0;

            // Act
            bool result = at24cEeprom.WriteByte(
                address,
                message);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WriteTest()
        {
            // Arrange
            IEeprom at24cEeprom = new At24cEeprom();
            byte address = 0;
            byte[] message = null;

            // Act
            bool result = at24cEeprom.Write(address, message);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WriteStringTest()
        {
            // Arrange
            IEeprom at24cEeprom = new At24cEeprom();
            byte address = 0;
            string message = null;

            // Act
            bool result = at24cEeprom.WriteString(address,message);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReadStringTest()
        {
            // Arrange
            IEeprom at24cEeprom = new At24cEeprom();
            byte address = 0;

            // Act
            string result = at24cEeprom.ReadString(address);

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
