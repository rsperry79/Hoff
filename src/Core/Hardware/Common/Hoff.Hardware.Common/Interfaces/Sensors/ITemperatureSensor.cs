using Hoff.Core.Hardware.Common.Interfaces.Events;
using System;

using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface ITemperatureSensor : ISensorBase, IDisposable
    {
        // Event Handlers
        internal delegate void TemperatureChangedEventHandler(object sender, ITemperatureChangedEvent tempatureChangedEvent);
        event EventHandler<ITemperatureChangedEvent> TemperatureChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        Temperature Temperature { get; }
    }
}
