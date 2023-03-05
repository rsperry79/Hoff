using Hoff.Core.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Core.Hardware.Common.Models
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
