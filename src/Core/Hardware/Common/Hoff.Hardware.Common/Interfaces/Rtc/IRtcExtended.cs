using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Rtc
{
    public interface IRtcExtended
    {
        #region Public Methods

        Temperature GetRtcTemperature();

        #endregion Public Methods
    }
}
