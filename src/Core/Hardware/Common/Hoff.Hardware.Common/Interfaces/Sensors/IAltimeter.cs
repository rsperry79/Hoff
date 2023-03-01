using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IAltimeter : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void AltimeterChangedEventHandler();
        event AltimeterChangedEventHandler AltimeterSensorChanged;

        /// <summary>
        /// The current temperature
        /// </summary>
        UnitsNet.Length Altitude { get; }
    }
}
