using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Common.Tests.Models
{
    [TestClass]
    public class PinConfigTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            int mosi = 11;
            int miso = 12;
            int clock = 13;

            int data = 14;
            int clock2 = 15;

            int data2 = 16;
            int clock3 = 17;

            IPinConfig pinConfig = new PinConfig
            {
                // Act
                Spi1_Clock = clock,
                Spi1_Miso = miso,
                Spi1_Mosi = mosi,

                I2C1_CLOCK = clock2,
                I2C1_DATA = data,

                I2C2_CLOCK = clock3,
                I2C2_DATA = data2
            };

            // Assert
            Assert.IsNotNull(pinConfig);

            // SPI 1
            Assert.AreEqual(clock, pinConfig.Spi1_Clock);
            Assert.AreEqual(miso, pinConfig.Spi1_Miso);
            Assert.AreEqual(mosi, pinConfig.Spi1_Mosi);

            // I2C 1
            Assert.AreEqual(clock2, pinConfig.I2C1_CLOCK);
            Assert.AreEqual(data, pinConfig.I2C1_DATA);

            // I2C 2
            Assert.AreEqual(clock3, pinConfig.I2C2_CLOCK);
            Assert.AreEqual(data2, pinConfig.I2C2_DATA);
        }
    }
}
