using System;

using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IHumiditySensor : ISensorBase, IDisposable
    {
        #region Delegates

        // Event Handlers
        public delegate void HumidityChangedEventHandler(object sender, IHumidityChangedEventArgs humidityChangedEvent);

        #endregion Delegates

        #region Events

        public event EventHandler<IHumidityChangedEventArgs> HumidityChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// The current Humidity level
        /// </summary>
        public RelativeHumidity Humidity { get; }

        #endregion Properties
    }
}
