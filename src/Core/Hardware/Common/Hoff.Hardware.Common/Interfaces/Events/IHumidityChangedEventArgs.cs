using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Events
{
    public interface IHumidityChangedEventArgs
    {
        RelativeHumidity RelativeHumidity { get; }
    }
}
