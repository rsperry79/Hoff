using System.Collections;
using System.Device.I2c;

namespace Hoff.Core.Hardware.Common.Interfaces.Services
{
    public interface II2cBussControllerService
    {
        #region Properties

        ArrayList I2C1 { get; }
        I2cBusSpeed I2C1BusSpeed { get; }
        ArrayList I2C2 { get; }
        I2cBusSpeed I2C2BusSpeed { get; }

        #endregion Properties

        #region Public Methods

        void SetIC2BussSpeed(int bussID, I2cBusSpeed speed);

        #endregion Public Methods
    }
}
