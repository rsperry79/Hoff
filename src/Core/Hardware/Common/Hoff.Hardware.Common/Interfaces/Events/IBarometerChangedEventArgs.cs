

using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Events
{
    public interface IBarometerChangedEventArgs
    {
        Pressure Pressure { get; }
    }
}
