using Hoff.Core.Hardware.Common.Interfaces.Config;

namespace Hoff.Hardware.SoC.SoCEsp32.Models
{
    public class PinConfig : IPinConfig
    {
        #region Properties

        public int I2C1_CLOCK { get; set; } = nanoFramework.Hardware.Esp32.Gpio.IO22;

        // I2C 1
        public int I2C1_DATA { get; set; } = nanoFramework.Hardware.Esp32.Gpio.IO21;

        public int I2C2_CLOCK { get; set; } = 33;

        // I2C 2
        public int I2C2_DATA { get; set; } = 32;

        public int Spi1_Clock { get; set; } = nanoFramework.Hardware.Esp32.Gpio.IO14;

        public int Spi1_Miso { get; set; } = nanoFramework.Hardware.Esp32.Gpio.IO12;

        // SPI 1
        public int Spi1_Mosi { get; set; } = nanoFramework.Hardware.Esp32.Gpio.IO13;

        #endregion Properties
    }
}
