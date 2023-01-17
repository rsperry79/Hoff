using Hoff.Hardware.Common.Interfaces.Config;

namespace Hoff.Hardware.Common.Models
{
    public class PinConfig : IPinConfig
    {
        // SPI 1
        public int Spi1_Mosi { get; set; } = 8;
        public int Spi1_Miso { get; set; } = 7;
        public int Spi1_Clock { get; set; } = 6;

        // I2C 1
        public int I2C1_DATA { get; set; } = 21;
        public int I2C1_CLOCK { get; set; } = 22;

        // I2C 2
        public int I2C2_DATA { get; set; } = 32;
        public int I2C2_CLOCK { get; set; } = 33;

    }
}
