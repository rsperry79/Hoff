

using System;
using System.Device.Spi;

using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.Max31865;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces
{
    public interface IMax31865Senor : ITemperatureSensor, ISensorBase, IDisposable
    {


        bool DefaultInit();
        bool Init(int bussId, int selectPin, SpiMode mode, PlatinumResistanceThermometerType thermometerType, ResistanceTemperatureDetectorWires wires, int resistance, uint scale);
    }
}
