using System.Device.I2c;

using Hoff.Core.Hardware.Common.Interfaces.Displays;

using static Iot.Device.Ssd13xx.Ssd13xx;

namespace Hoff.Hardware.Displays.Ssd13.Interfaces
{
    public interface ISsd13 : IDisplay
    {
        #region Public Methods

        bool DefaultInit();

        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, DisplayResolution resolution);

        #endregion Public Methods
    }
}
