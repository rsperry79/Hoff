﻿using Hoff.Hardware.Common.Interfaces.Events;

using UnitsNet;

namespace Hoff.Hardware.Common.Models
{


    public class BarometerChangedEventArgs : IBarometerChangedEventArgs
    {
        public BarometerChangedEventArgs(Pressure pressure) => this.Pressure = pressure;

        public Pressure Pressure { get; private set; }
    }
}
