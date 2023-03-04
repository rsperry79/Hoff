using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface ITemperatureChangedEvent
    {
        Temperature Temperature { get; }
    }
}