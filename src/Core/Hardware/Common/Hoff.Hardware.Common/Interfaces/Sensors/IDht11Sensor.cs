
using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IDht11Sensor : IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        void Init(int pin, uint scale = 2);
    }
}
