namespace Hoff.Hardware.Common.Interfaces.Config
{
    public interface IPinConfig
    {
        int Spi1_Clock { get; set; }
        int Spi1_Miso { get; set; }
        int Spi1_Mosi { get; set; }
        int I2C1_DATA { get; set; }
        int I2C1_CLOCK { get; set; }
        int I2C2_DATA { get; set; }
        int I2C2_CLOCK { get; set; }
    }
}
