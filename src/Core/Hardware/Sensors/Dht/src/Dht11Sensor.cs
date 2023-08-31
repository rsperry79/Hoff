using System;
using System.Diagnostics;

using Hoff.Core.Hardware.Common.Helpers;
using Hoff.Core.Hardware.Senors.Common.Abstract;
using Hoff.Core.Hardware.Sensors.Dht.Interfaces;
using Hoff.Hardware.Common.Senors.Interfaces;
using Hoff.Hardware.Common.Senors.Interfaces.Base;
using Hoff.Hardware.Common.Senors.Interfaces.Events;
using Hoff.Hardware.Common.Senors.Models;

using Iot.Device.DHTxx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.Dht
{
    public class Dht11Sensor : SensorBase, IDht11Sensor, ITemperatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Fields

        private static DhtBase Dht = null;

        private readonly object locker;
        private readonly ILogger logger;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private uint _scale = 2;

        private bool disposedValue;

        /// <summary>
        /// Accessors/Mutator for relative humidity %
        /// </summary>
        private RelativeHumidity relativeHumidity;

        private Temperature temperature;

        #endregion Fields

        #region Public Constructors

        public Dht11Sensor() => this.logger = this.GetCurrentClassLogger();

        #endregion Public Constructors

        #region Private Destructors

        ~Dht11Sensor()
        {
            this.Dispose(disposing: false);
        }

        #endregion Private Destructors

        #region Events

        /// <summary>
        /// Humidity changed event handler.
        /// </summary>
        public event EventHandler<IHumidityChangedEventArgs> HumidityChanged;

        /// <summary>
        /// Temperature Changed Event handler
        /// </summary>
        public event EventHandler<ITemperatureChangedEvent> TemperatureChanged;

        #endregion Events

        #region Properties

        public RelativeHumidity Humidity
        {
            get => this.relativeHumidity;
            private set
            {
                if (this.relativeHumidity.Percent.Truncate(this._scale) != value.Percent.Truncate(this._scale))
                {
                    this.relativeHumidity = value;

                    EventHandler<IHumidityChangedEventArgs> tempEvent = HumidityChanged;
                    tempEvent(this, new HumidityChangedEventArgs(value));
                }
            }
        }

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
        /// This sensor supports change tracking
        /// </summary>
        /// <returns>bool</returns>
        public override bool CanTrackChanges()
        {
            return true;
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Init(int pin, uint scale = 2)
        {
            Dht = new Dht11(pin);

            this._scale = scale;
            this.logger.LogTrace("before sleep");
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
            Dht.Dispose();
        }

        /// <summary>
        /// Let the world know whether the sensor value has changed or not
        /// </summary>
        /// <returns>bool</returns>
        protected override void RefreshSenorData()
        {
            lock (this.locker)
            {
                try
                {
                    this.temperature = this.ReadTemperature();
                    this.Humidity = this.ReadHumidity();
                }
                catch (Exception)
                {
                    Debug.WriteLine("HasSensorValueChanged");
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private RelativeHumidity ReadHumidity()
        {
            try
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return Dht.Humidity;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ReadHumidity");
            }

            return default;
        }

        private Temperature ReadTemperature()
        {
            try
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return Dht.Temperature;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ReadHumidity");
            }

            return default;
        }

        #endregion Private Methods
    }
}
