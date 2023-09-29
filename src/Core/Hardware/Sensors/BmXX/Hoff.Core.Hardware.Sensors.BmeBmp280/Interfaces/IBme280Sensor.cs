using System;
using System.Device.I2c;

using Hoff.Hardware.Common.Senors.Interfaces;
using Hoff.Hardware.Common.Senors.Interfaces.Base;

namespace Hoff.Core.Hardware.Sensors.BmXX.Interfaces
{
    public interface IBme280Sensor : ITemperatureSensor, IHumiditySensor, IBarometer, IAltimeter, ISensorBase, IDisposable
    {
        #region Public Methods

        bool DefaultInit();

        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale);

        void Reset();

        #endregion Public Methods
    }
}
