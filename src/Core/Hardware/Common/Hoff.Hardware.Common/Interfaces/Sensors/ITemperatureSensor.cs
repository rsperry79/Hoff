using System;

using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface ITemperatureSensor : ISensorBase, IDisposable
    {
        #region Delegates

        // Event Handlers
        internal delegate void TemperatureChangedEventHandler(object sender, ITemperatureChangedEvent tempatureChangedEvent);

        #endregion Delegates

        #region Events

        event EventHandler<ITemperatureChangedEvent> TemperatureChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// The current temperature
        /// </summary>
        Temperature Temperature { get; }

        #endregion Properties
    }
}