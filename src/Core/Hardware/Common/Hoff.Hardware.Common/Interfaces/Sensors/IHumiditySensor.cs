using Hoff.Core.Hardware.Common.Interfaces.Events;
using System;

using Hoff.Hardware.Common.Interfaces.Base;

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Sensors
{
    public interface IHumiditySensor : ISensorBase, IDisposable
    {
        // Event Handlers
        public delegate void HumidityChangedEventHandler(object sender, IHumidityChangedEventArgs humidityChangedEvent);
        public event EventHandler<IHumidityChangedEventArgs> HumidityChanged;

        /// <summary>
        /// The current Humidity level
        /// </summary>
        public RelativeHumidity Humidity { get; }
    }
}
