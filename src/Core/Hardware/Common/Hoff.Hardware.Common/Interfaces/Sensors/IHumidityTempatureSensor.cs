
using System;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IHumidityTempatureSensor : IHumiditySensor, ITempatureSensor, IDisposable
    {
    }
}
