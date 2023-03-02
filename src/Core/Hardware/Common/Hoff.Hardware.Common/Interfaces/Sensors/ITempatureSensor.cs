using System;

using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface ITempatureSensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void TempatureChangedEventHandler(object sender, ITempatureChangedEventArgs tempatureChangedEvent);
        event TempatureChangedEventHandler TemperatureChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        Temperature Temperature { get; }
    }
}
