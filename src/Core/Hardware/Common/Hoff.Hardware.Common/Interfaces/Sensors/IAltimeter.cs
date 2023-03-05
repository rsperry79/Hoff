using System;

using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IAltimeter : ISensorBase, IDisposable
    {
        // Event Handlers

        #region Delegates

        public delegate void AltimeterChangedEventHandler(object sender, IAltimeterChangedEventArgs altimeterChangedEvent);

        #endregion Delegates

        #region Events

        public event EventHandler<IAltimeterChangedEventArgs> AltimeterChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// The current temperature
        /// </summary>
        Length Altitude { get; }

        #endregion Properties
    }
}
