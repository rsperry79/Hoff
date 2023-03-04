using Hoff.Core.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Models
{

    public class TemperatureChangedEvent : ITemperatureChangedEvent
    {
        public TemperatureChangedEvent(Temperature temperature) => this.Temperature = temperature;

        public Temperature Temperature { get; private set; }
    }
}
