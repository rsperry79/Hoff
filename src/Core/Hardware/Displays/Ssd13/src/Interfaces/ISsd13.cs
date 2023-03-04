﻿using Hoff.Core.Hardware.Common.Interfaces.Displays;

using System.Device.I2c;


using static Iot.Device.Ssd13xx.Ssd13xx;

namespace Hoff.Hardware.Displays.Ssd13.Interfaces
{
    public interface ISsd13 : IDisplay
    {
        bool DefaultInit();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, DisplayResolution resolution);

    }
}
