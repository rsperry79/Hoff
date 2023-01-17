﻿using nanoFramework.Hardware.Esp32;

namespace Hoff.Hardware.Common.Interfaces.Services
{
    public interface IEspConfig
    {
        int GetPinFunction(DeviceFunction deviceFunction);
        void SetI2C1Pins();
        void SetI2C2Pins();
        void SetSpi1Pins();
    }

}
