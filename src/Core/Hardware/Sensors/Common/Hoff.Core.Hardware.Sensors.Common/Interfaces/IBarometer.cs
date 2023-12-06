using System;

using Hoff.Hardware.Common.Senors.Interfaces.Base;
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces
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