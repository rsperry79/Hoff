
using Hoff.Hardware.Common.Senors.Interfaces.Events;

using UnitsNet;


namespace Hoff.Hardware.Common.Senors.Models
{
    public class HumidityChangedEventArgs : IHumidityChangedEventArgs
    {
        #region Public Constructors

        public HumidityChangedEventArgs(RelativeHumidity relativeHumidity) => this.RelativeHumidity = relativeHumidity;

        #endregion Public Constructors

        #region Properties

        public RelativeHumidity RelativeHumidity { get; private set; }

        #endregion Properties
    }
}
