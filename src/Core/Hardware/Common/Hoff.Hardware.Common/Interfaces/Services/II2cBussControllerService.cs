
using System.Collections;
using System.Device.I2c;

namespace Hoff.Core.Hardware.Common.Interfaces.Services
{
    public interface II2cBussControllerService
    {
        ArrayList I2C1 { get; }
        ArrayList I2C2 { get; }
        I2cBusSpeed I2C1BusSpeed { get; }
        I2cBusSpeed I2C2BusSpeed { get; }

        void SetIC2BussSpeed(int bussID, I2cBusSpeed speed);
    }
}
