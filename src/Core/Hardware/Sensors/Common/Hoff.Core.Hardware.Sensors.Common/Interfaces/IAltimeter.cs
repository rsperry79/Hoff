using System;

using Hoff.Hardware.Common.Senors.Interfaces.Base;
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces
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
