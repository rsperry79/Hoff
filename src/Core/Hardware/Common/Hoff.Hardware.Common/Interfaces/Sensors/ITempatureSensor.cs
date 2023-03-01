using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface ITempatureSensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void TempatureChangedEventHandler();
        event TempatureChangedEventHandler TemperatureSensorChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        UnitsNet.Temperature Temperature { get; }
    }
}
