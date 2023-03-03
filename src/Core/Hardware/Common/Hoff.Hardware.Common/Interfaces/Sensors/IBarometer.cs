using System;

using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IBarometer : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void BarometerChangedEventHandler(object sender, IBarometerChangedEventArgs humidityChangedEvent);
        event EventHandler<IBarometerChangedEventArgs> PressureChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        Pressure Pressure { get; }
    }
}
