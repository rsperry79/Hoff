using System;

using Hoff.Hardware.Common.Senors.Interfaces.Base;
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces
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
