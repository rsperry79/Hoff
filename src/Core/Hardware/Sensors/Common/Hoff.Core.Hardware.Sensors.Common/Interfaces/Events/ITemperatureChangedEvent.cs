using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces.Events
{
    public interface ITemperatureChangedEvent
    {
        #region Properties

        Temperature Temperature { get; }

        #endregion Properties
    }
}
