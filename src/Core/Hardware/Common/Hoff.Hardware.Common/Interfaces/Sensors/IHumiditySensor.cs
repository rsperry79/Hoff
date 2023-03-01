using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IHumiditySensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate bool HumidityChangedEventHandler();
        event HumidityChangedEventHandler HumiditySensorChanged;

        /// <summary>
        /// The current Humidity level
        /// </summary>
        double Humidity { get; }
    }
}
