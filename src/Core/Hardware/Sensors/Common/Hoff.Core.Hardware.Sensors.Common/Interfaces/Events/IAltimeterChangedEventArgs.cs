using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces.Events
{
    public interface IAltimeterChangedEventArgs
    {
        #region Properties

        Length Altitude { get; }

        #endregion Properties
    }
}
