using System;
using System.Device.I2c;

using Hoff.Core.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

namespace Hoff.Core.Hardware.Sensors.BmXX.Interfaces
{
    public interface IBme280Sensor : IHumidityTemperatureSensor, ITemperatureSensor, IHumiditySensor, IBarometer, IAltimeter, ISensorBase, IDisposable
    {
        #region Public Methods

        bool DefaultInit();

        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale);

        void Reset();

        #endregion Public Methods
    }
}
