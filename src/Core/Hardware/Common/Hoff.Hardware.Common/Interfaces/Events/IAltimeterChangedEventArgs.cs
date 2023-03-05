using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Events
{
    public interface IAltimeterChangedEventArgs
    {
        #region Properties

        Length Altitude { get; }

        #endregion Properties
    }
}
