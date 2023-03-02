using System;

using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IAltimeter : ISensorBase, IDisposable
    {
        // Event Handlers

        public delegate void AltimeterChangedEventHandler(object sender, IAltimeterChangedEventArgs altimeterChangedEvent);
        public event EventHandler<IAltimeterChangedEventArgs> AltimeterChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        Length Altitude { get; }
    }
}
