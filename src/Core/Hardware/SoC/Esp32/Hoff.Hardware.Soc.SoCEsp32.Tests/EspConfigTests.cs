
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Soc.SoCEsp32.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.Hardware.Esp32;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Soc.SoCEsp32.Tests
{
    [TestClass]
    public class EspConfigTests
    {
        private static ServiceProvider services;

        private static IPinConfig pins;

        [Setup]
        public void Setup()
        {

            services = DiSetup.ConfigureServices();
            pins = (IPinConfig)services.GetRequiredService(typeof(IPinConfig));

        }

        [TestMethod]
        public void SetSpiPinsTest()
        {
            // Arrange
            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));

            // Act
            espConfig.SetSpi1Pins();
            // Assert
            Assert.AreEqual(pins.Spi1_Clock, espConfig.GetPinFunction(DeviceFunction.SPI1_CLOCK));
            Assert.AreEqual(pins.Spi1_Miso, espConfig.GetPinFunction(DeviceFunction.SPI1_MISO));
            Assert.AreEqual(pins.Spi1_Mosi, espConfig.GetPinFunction(DeviceFunction.SPI1_MOSI));

        }


        [TestMethod]
        public void SetI2C1PinsTest()
        {
            // Arrange
            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));


            // Act
            espConfig.SetI2C1Pins();

            // Assert
            Assert.AreEqual(pins.I2C1_CLOCK, espConfig.GetPinFunction(DeviceFunction.I2C1_CLOCK));
            Assert.AreEqual(pins.I2C1_DATA, espConfig.GetPinFunction(DeviceFunction.I2C1_DATA));
        }

        [TestMethod]
        public void SetI2C2PinsTest()
        {
            // Arrange
            ServiceProvider services = DiSetup.ConfigureServices();
            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));

            // Act
            espConfig.SetI2C2Pins();

            // Assert
            Assert.AreEqual(pins.I2C2_CLOCK, espConfig.GetPinFunction(DeviceFunction.I2C2_CLOCK));
            Assert.AreEqual(pins.I2C2_DATA, espConfig.GetPinFunction(DeviceFunction.I2C2_DATA));
        }
    }
}
