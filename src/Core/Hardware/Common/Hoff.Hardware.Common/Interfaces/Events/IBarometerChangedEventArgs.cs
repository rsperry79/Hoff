

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IBarometerChangedEventArgs
    {
        Pressure Pressure { get; }
    }
}
