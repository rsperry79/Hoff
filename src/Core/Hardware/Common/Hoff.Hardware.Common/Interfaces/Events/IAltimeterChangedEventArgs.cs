
using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IAltimeterChangedEventArgs
    {
        Length Altitude { get; }
    }

}
