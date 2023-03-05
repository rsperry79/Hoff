namespace Hoff.Core.Hardware.Common.Interfaces.Config
{
    public interface IPinConfig
    {
        #region Properties

        int I2C1_CLOCK { get; set; }
        int I2C1_DATA { get; set; }
        int I2C2_CLOCK { get; set; }
        int I2C2_DATA { get; set; }
        int Spi1_Clock { get; set; }
        int Spi1_Miso { get; set; }
        int Spi1_Mosi { get; set; }

        #endregion Properties
    }
}
