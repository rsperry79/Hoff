using System;

using Hoff.Hardware.Common.Senors.Interfaces.Base;
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces
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