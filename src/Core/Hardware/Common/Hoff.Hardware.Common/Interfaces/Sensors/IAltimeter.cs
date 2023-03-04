using Hoff.Core.Hardware.Common.Interfaces.Events;
using System;

using Hoff.Hardware.Common.Interfaces.Base;

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
