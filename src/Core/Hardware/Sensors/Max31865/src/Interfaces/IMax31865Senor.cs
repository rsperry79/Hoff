using System;
using System.Device.Spi;

using Hoff.Hardware.Common.Senors.Interfaces;
using Hoff.Hardware.Common.Senors.Interfaces.Base;

using Iot.Device.Max31865;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces
{
    public interface IMax31865Senor : ITemperatureSensor, ISensorBase, IDisposable
    {
        #region Public Methods

        bool DefaultInit();

        bool Init(int bussId, int selectPin, SpiMode mode, PlatinumResistanceThermometerType thermometerType, ResistanceTemperatureDetectorWires wires, int resistance, uint scale);

        #endregion Public Methods
    }
}
