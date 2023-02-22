using System;
using System.Device.Spi;

using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Base;

using Iot.Device.Max31865;

using UnitsNet;

namespace Hoff.Hardware.Sensors.Max31865Senor

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
        private double temperature;
        private bool disposedValue;

        /// <summary>
        /// Accessor/Mutator for temperature in celcius
        /// </summary>
        public double Temperature
        {
            get => this.temperature;
            set
            {
                if (this.temperature != value)
                {
                    this.temperature = value;
                    TemperatureSensorChanged();
                }

            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Tempature Changed Event handler
        /// </summary>
        public event ITempatureSensor.TempatureChangedEventHandler TemperatureSensorChanged;
        #endregion

        #region Constructor
        public Max31865Senor(uint scale = 2)
        {
            this._scale = scale;

            SpiConnectionSettings settings = new(1, 42)
            {
                ClockFrequency = Max31865.SpiClockFrequency,
                Mode = Max31865.SpiMode1,
                DataFlow = Max31865.SpiDataFlow
            };

            this.spiDevice = SpiDevice.Create(settings);
            this.sensor = new(this.spiDevice, PlatinumResistanceThermometerType.Pt1000, ResistanceTemperatureDetectorWires.TwoWire, ElectricResistance.FromOhms(4300));


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

            float temp = ((float)this.ReadTemperature().DegreesCelsius).Truncate(this._scale);
            this.Temperature = temp;
        }

        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            this.sensor.Dispose();
            this.spiDevice.Dispose();
            this.spiDevice = null;
            this.sensor = null;
        }

        ~Max31865Senor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Helpers
        private Temperature ReadTemperature()
        {
            return this.sensor.Temperature;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (!this._disposed)
                    {
                        this.DisposeSensor();
                        this._disposed = true;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this.disposedValue = true;
            }
        }


        #endregion
    }
}
