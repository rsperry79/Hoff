using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Models
{
    public class AltimeterChangedEventArgs : IAltimeterChangedEventArgs
    {
        public AltimeterChangedEventArgs(Length altitude) => this.Altitude = altitude;

        public Length Altitude { get; private set; }
    }
}
