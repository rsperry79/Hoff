using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface ITemperatureChangedEvent
    {
        #region Properties

        Temperature Temperature { get; }

        #endregion Properties
    }
}
