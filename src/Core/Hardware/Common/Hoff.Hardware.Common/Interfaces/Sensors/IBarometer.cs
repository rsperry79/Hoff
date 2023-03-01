using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IBarometer : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void PressureChangedEventHandler();
        event PressureChangedEventHandler PressureSensorChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        UnitsNet.Pressure Pressure { get; }
    }
}
