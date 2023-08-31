using System;

using Hoff.Hardware.Common.Senors.Interfaces;
using Hoff.Hardware.Common.Senors.Interfaces.Base;

namespace Hoff.Core.Hardware.Sensors.Dht.Interfaces
{
    public interface IDht11Sensor :ITemperatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Public Methods

        void Init(int pin, uint scale = 2);

        #endregion Public Methods
    }
}
