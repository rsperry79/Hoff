
using System;
using System.Device.I2c;

namespace Hoff.Hardware.Common.Interfaces.Rtc
{
    public interface IRtc
    {
        bool DefaultInit();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed);
        DateTime ReadDateTime();
        void SetDateTime(DateTime time);
    }

}
