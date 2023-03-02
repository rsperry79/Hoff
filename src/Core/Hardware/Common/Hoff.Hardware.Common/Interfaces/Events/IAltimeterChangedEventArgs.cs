
using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Events
{
    public interface IAltimeterChangedEventArgs
    {
        Length Altitude { get; }
    }

}
