
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Models
{
    public class AltimeterChangedEventArgs : IAltimeterChangedEventArgs
    {
        #region Public Constructors

        public AltimeterChangedEventArgs(Length altitude) => this.Altitude = altitude;

        #endregion Public Constructors

        #region Properties

        public Length Altitude { get; private set; }

        #endregion Properties
    }
}
