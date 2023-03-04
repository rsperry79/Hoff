using UnitsNet;

namespace Hoff.Core.Hardware.Common.Interfaces.Rtc
{
    public interface IRtcExtended
    {
        Temperature GetRtcTemperature();
    }
}
