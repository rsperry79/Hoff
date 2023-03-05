using System;
using System.Device.I2c;

namespace Hoff.Core.Hardware.Common.Interfaces.Rtc
{
    public interface IRtc
    {
        #region Public Methods

        bool DefaultInit();

        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed);

        DateTime ReadDateTime();

        void SetDateTime(DateTime time);

        #endregion Public Methods
    }
}
