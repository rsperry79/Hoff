using Hoff.Core.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Models
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
