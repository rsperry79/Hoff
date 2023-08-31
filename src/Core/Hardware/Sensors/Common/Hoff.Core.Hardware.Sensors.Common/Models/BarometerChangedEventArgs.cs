using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Models
{
    public class BarometerChangedEventArgs : IBarometerChangedEventArgs
    {
        #region Public Constructors

        public BarometerChangedEventArgs(Pressure pressure) => this.Pressure = pressure;

        #endregion Public Constructors

        #region Properties

        public Pressure Pressure { get; private set; }

        #endregion Properties
    }
}
