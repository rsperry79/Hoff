using UnitsNet;

namespace Hoff.Hardware.Common.Senors.Interfaces.Events
{
    public interface IBarometerChangedEventArgs
    {
        #region Properties

        Pressure Pressure { get; }

        #endregion Properties
    }
}
