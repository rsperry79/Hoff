

using System;
using System.Device.I2c;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IBme280Sensor : IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        bool DefaultInit();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale);
        void Reset();
    }

}
