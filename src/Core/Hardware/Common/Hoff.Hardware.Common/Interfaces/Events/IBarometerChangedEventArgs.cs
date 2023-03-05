using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IBarometerChangedEventArgs
    {
        #region Properties

        Pressure Pressure { get; }

        #endregion Properties
    }
}
