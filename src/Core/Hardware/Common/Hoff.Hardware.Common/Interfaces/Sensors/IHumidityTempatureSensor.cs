using System;

using Hoff.Hardware.Common.Interfaces.Sensors;

namespace Hoff.Core.Hardware.Common.Interfaces.Sensors
{
    public interface IHumidityTemperatureSensor : IHumiditySensor, ITemperatureSensor, IDisposable
    {
    }
}
