

using System;
using System.Device.I2c;

using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

namespace Hoff.Core.Hardware.Sensors.BmXX.Interfaces
{
    public interface IBme280Sensor : IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, IBarometer, IAltimeter, ISensorBase, IDisposable
    {
        bool DefaultInit();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale);
        void Reset();
    }

}
