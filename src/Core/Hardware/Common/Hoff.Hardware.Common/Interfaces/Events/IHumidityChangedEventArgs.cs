using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IHumidityChangedEventArgs
    {
        #region Properties

        RelativeHumidity RelativeHumidity { get; }

        #endregion Properties
    }
}
