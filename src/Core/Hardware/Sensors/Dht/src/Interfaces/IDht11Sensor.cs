
using System;

using Hoff.Core.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

namespace Hoff.Core.Hardware.Sensors.Dht.Interfaces
{
    public interface IDht11Sensor : IHumidityTemperatureSensor, ITemperatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        void Init(int pin, uint scale = 2);
    }
}
