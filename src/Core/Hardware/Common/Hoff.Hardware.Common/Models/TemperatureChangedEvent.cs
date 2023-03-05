using Hoff.Core.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Models
{
    public class TemperatureChangedEvent : ITemperatureChangedEvent
    {
        #region Public Constructors

        public TemperatureChangedEvent(Temperature temperature) => this.Temperature = temperature;

        #endregion Public Constructors

        #region Properties

        public Temperature Temperature { get; private set; }

        #endregion Properties
    }
}
