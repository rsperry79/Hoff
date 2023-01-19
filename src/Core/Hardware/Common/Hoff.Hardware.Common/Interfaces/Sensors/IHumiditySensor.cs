using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces
{
    public interface IHumiditySensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate bool HumidityChangedEventHandler();
        event HumidityChangedEventHandler HumiditySensorChanged;

        /// <summary>
        /// The current Humidity level
        /// </summary>
        UnitsNet.RelativeHumidity Humidity { get; }
    }
}
