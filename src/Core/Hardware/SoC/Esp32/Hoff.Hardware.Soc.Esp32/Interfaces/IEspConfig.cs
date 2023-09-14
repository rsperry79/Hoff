using nanoFramework.Hardware.Esp32;

namespace Hoff.Hardware.SoC.SoCEsp32.Interfaces
{
    public interface IEspConfig
    {
        #region Public Methods

        int GetPinFunction(DeviceFunction deviceFunction);

        void SetI2C1Pins();

        void SetI2C2Pins();

        void SetSpi1Pins();

        #endregion Public Methods
    }
}
