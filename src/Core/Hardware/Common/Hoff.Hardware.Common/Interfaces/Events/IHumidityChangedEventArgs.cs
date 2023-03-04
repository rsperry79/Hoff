using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IHumidityChangedEventArgs
    {
        RelativeHumidity RelativeHumidity { get; }
    }
}
