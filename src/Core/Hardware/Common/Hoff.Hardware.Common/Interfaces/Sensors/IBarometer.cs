using System;

using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IBarometer : ISensorBase, IDisposable
    {
        #region Delegates

        // Event Handlers
        delegate void BarometerChangedEventHandler(object sender, IBarometerChangedEventArgs humidityChangedEvent);

        #endregion Delegates

        #region Events

        event EventHandler<IBarometerChangedEventArgs> PressureChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// The current temperature
        /// </summary>
        Pressure Pressure { get; }

        #endregion Properties
    }
}