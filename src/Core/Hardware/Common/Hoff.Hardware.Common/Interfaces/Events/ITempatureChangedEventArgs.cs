
using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Events
{
    public interface ITempatureChangedEventArgs
    {
        Temperature Temperature { get; }
    }
}
