

using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;


namespace Hoff.Hardware.Common.Senors.Models
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
