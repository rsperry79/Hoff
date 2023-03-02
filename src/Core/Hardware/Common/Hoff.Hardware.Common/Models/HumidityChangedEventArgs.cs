using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Models
{
    public class HumidityChangedEventArgs : IHumidityChangedEventArgs
    {
        public HumidityChangedEventArgs(RelativeHumidity relativeHumidity) => this.RelativeHumidity = relativeHumidity;

        public RelativeHumidity RelativeHumidity { get; private set; }
    }
}
