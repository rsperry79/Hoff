
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Soc.SoCEsp32.Tests.Helpers;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

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
        private static IEspConfig espConfig;

        [Setup]
        public void Setup()
        {

            services = DiSetup.ConfigureServices();
            pins = (IPinConfig)services.GetRequiredService(typeof(IPinConfig));
            espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));

        }

        [TestMethod]
        public void SetSpiPinsTest()
        {
            // Arrange

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

            // Act
            espConfig.SetI2C2Pins();

            // Assert
            Assert.AreEqual(pins.I2C2_CLOCK, espConfig.GetPinFunction(DeviceFunction.I2C2_CLOCK));
            Assert.AreEqual(pins.I2C2_DATA, espConfig.GetPinFunction(DeviceFunction.I2C2_DATA));
        }
    }
}
