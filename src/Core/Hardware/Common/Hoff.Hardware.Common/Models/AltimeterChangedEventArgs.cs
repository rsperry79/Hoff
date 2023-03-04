using Hoff.Core.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Models
{
    public class AltimeterChangedEventArgs : IAltimeterChangedEventArgs
    {
        public AltimeterChangedEventArgs(Length altitude) => this.Altitude = altitude;

        public Length Altitude { get; private set; }
    }
}
