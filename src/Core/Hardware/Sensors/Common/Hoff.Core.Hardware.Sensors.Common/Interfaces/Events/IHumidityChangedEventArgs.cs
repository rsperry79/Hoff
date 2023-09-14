using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces.Events
{
    public interface IHumidityChangedEventArgs
    {
        #region Properties

        RelativeHumidity RelativeHumidity { get; }

        #endregion Properties
    }
}
