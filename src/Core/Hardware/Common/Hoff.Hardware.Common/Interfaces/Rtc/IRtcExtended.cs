using UnitsNet;

namespace Hoff.Hardware.Common.Interfaces.Rtc
{
    public interface IRtcExtended
    {
        Temperature GetRtcTemperature();
    }
}
