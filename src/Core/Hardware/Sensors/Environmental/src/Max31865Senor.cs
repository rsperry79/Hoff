using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces;
using Iot.Device.Max31865;
using System;
using System.Device.Spi;
using UnitsNet;

namespace Hoff.Hardware.Sensors.Environmental
{
    public class Max31865Senor : SensorBase, ITempatureSensor, ISensorBase, IDisposable
    {

        #region Implementation
        private Max31865 sensor = null;

        private SpiDevice spiDevice;
        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private readonly uint _scale = 2;
        #endregion
        #region Properties
        private Temperature temperature;
        private bool disposedValue;

        /// <summary>
        /// Accessor/Mutator for temperature in celcius
        /// </summary>
        public Temperature Temperature
        {
            get => temperature;
            set
            {
                if (temperature.DegreesCelsius != value.DegreesCelsius)
                {
                    temperature = value;
                    TempatureSensorChanged();
                }

            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Tempature Changed Event handler
        /// </summary>
        public event ITempatureSensor.TempatureChangedEventHandler TempatureSensorChanged;
        #endregion

        #region Constructor
        public Max31865Senor(uint scale = 2)
        {
            SpiConnectionSettings settings = new(1, 42)
            {
                ClockFrequency = Max31865.SpiClockFrequency,
                Mode = Max31865.SpiMode1,
                DataFlow = Max31865.SpiDataFlow
            };

            spiDevice = SpiDevice.Create(settings);
            sensor = new(spiDevice, PlatinumResistanceThermometerType.Pt1000, ResistanceTemperatureDetectorWires.ThreeWire, ElectricResistance.FromOhms(4300));


            _scale = scale;
        }
        #endregion

        #region Change tracking
        /// <summary>
        /// This sensor suports change tracking
        /// </summary>
        /// <returns>bool</returns>
        public override bool CanTrackChanges()
        {
            return true;
        }

        /// <summary>
        /// Let the world know whether the sensor value has changed or not
        /// </summary>
        /// <returns>bool</returns>
        public override void HasSensorValueChanged()
        {

            float temp = ((float)ReadTemperature().DegreesCelsius).Truncate(_scale);
            Temperature = UnitsNet.Temperature.FromDegreesCelsius(temp);
        }

        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            sensor.Dispose();
            spiDevice.Dispose();
            spiDevice = null;
            sensor = null;
        }

        ~Max31865Senor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Helpers
        private Temperature ReadTemperature()
        {
            return sensor.Temperature;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!_disposed)
                    {
                        DisposeSensor();
                        _disposed = true;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }


        #endregion
    }
}
