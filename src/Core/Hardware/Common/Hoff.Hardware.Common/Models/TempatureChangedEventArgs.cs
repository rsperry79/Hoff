using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Models
{

    public class TempatureChangedEventArgs : ITempatureChangedEventArgs
    {
        public TempatureChangedEventArgs(Temperature temperature) => this.Temperature = temperature;

        public Temperature Temperature { get; private set; }
    }
}
