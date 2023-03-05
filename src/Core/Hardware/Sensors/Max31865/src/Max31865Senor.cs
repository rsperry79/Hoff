using System;
using System.Device.Spi;

using Hoff.Core.Hardware.Common.Abstract;
using Hoff.Core.Hardware.Common.Helpers;
using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.Max31865;

using nanoFramework.Logging;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor

{
    public class Max31865Senor : SensorBase, IMax31865Senor, ITemperatureSensor, ISensorBase, IDisposable
    {
        #region Fields

        private const int sensorSleepTime = 10;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private readonly uint _scale = 2;

        private readonly object locker;
        private bool disposedValue;
        private bool init;
        private Max31865 sensor = null;
        private SpiDevice spiDevice;
        private Temperature temperature;

        #endregion Fields

        #region Public Constructors

        public Max31865Senor() => this._logger = this.GetCurrentClassLogger();

        #endregion Public Constructors

        #region Private Destructors

        ~Max31865Senor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: false);
        }

        #endregion Private Destructors

        #region Events

        /// <summary>
        /// Tempature Changed Event handler
        /// </summary>
        public event EventHandler<ITemperatureChangedEvent> TemperatureChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Accessor/Mutator for temperature in Celsius
        /// </summary>
        public Temperature Temperature
        {
            get => this.temperature;
            private set
            {
                if (this.temperature.DegreesCelsius.Truncate(this._scale) != value.DegreesCelsius.Truncate(this._scale))
                {
                    this.temperature = value;

                    EventHandler<ITemperatureChangedEvent> tempEvent = TemperatureChanged;
                    tempEvent(this, new TemperatureChangedEvent(value));
                }
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// This sensor suports change tracking
        /// </summary>
        /// <returns>bool</returns>
        public override bool CanTrackChanges()
        {
            return true;
        }

        public bool DefaultInit()
        {
            int bussId = 1;
            int selectPin = 15;
            SpiMode spiMode = SpiMode.Mode3;
            PlatinumResistanceThermometerType thermometerType = PlatinumResistanceThermometerType.Pt100;
            ResistanceTemperatureDetectorWires wires = ResistanceTemperatureDetectorWires.TwoWire;
            int resistance = 4300;
            uint scale = 2;

            return this.Init(bussId, selectPin, spiMode, thermometerType, wires, resistance, scale);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool Init(
                    int bussId,
            int selectPin,
            SpiMode mode,
            PlatinumResistanceThermometerType thermometerType,
            ResistanceTemperatureDetectorWires wires,
            int resistance,
            uint scale)
        {
            if (!this.init)
            {
                if (mode != SpiMode.Mode1 || mode != SpiMode.Mode3)
                {
                    throw new ArgumentException(nameof(SpiMode));
                }

                SpiConnectionSettings settings = new(1, 42)
                {
                    ClockFrequency = Max31865.SpiClockFrequency,
                    Mode = mode,
                    DataFlow = Max31865.SpiDataFlow
                };

                this.spiDevice = SpiDevice.Create(settings);
                this.sensor = new(this.spiDevice, thermometerType, wires, ElectricResistance.FromOhms(resistance));

                this.init = true;
            }

            return this.init;
        }

        #endregion Public Methods

        #region Protected Methods

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

        protected override void DisposeSensor()
        {
            this.sensor.Dispose();
            this.spiDevice.Dispose();
            this.spiDevice = null;
            this.sensor = null;
        }

        /// <summary>
        /// Let the world know whether the sensor value has changed or not
        /// </summary>
        /// <returns>bool</returns>
        protected override void RefreshSenorData()
        {
            lock (this.locker)
            {
                this.Temperature = this.sensor.Temperature;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private Temperature ReadTemperature()
        {
            return this.sensor.Temperature;
        }

        #endregion Private Methods
    }
}
