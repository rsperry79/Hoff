using Hoff.Core.Hardware.Common.Abstract;
using System;
using System.Diagnostics;

using Hoff.Core.Hardware.Sensors.Dht.Interfaces;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.DHTxx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using UnitsNet;
using Hoff.Core.Hardware.Common.Helpers;
using Hoff.Core.Hardware.Common.Interfaces.Events;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Hardware.Common.Interfaces.Sensors;

namespace Hoff.Core.Hardware.Sensors.Dht
{
    public class Dht11Sensor : SensorBase, IDht11Sensor, IHumidityTemperatureSensor, ITemperatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Implementation
        private static DhtBase Dht = null;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private uint _scale = 2;

        private readonly ILogger logger;

        public Dht11Sensor() => this.logger = this.GetCurrentClassLogger();
        #endregion

        #region Properties
        /// <summary>
        /// Accessors/Mutator for relative humidity %
        /// </summary>
        private RelativeHumidity relativeHumidity;

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

        private Temperature temperature;

        private bool disposedValue;

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

        #endregion

        #region Event Handlers
        /// <summary>
        /// Temperature Changed Event handler
        /// </summary>
        public event EventHandler<ITemperatureChangedEvent> TemperatureChanged;

        /// <summary>
        /// Humidity changed event handler.
        /// </summary>
        public event EventHandler<IHumidityChangedEventArgs> HumidityChanged;

        #endregion

        #region Constructor

        public void Init(int pin, uint scale = 2)
        {
            Dht = new Dht11(pin);

            this._scale = scale;
            this.logger.LogTrace("before sleep");
        }
        #endregion

        #region Change tracking
        /// <summary>
        /// This sensor supports change tracking
        /// </summary>
        /// <returns>bool</returns>
        public override bool CanTrackChanges()
        {
            return true;
        }

        private readonly object locker;
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
        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            Dht.Dispose();
        }

        ~Dht11Sensor()
        {
            this.Dispose(disposing: false);
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

        #region Helpers
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
        #endregion
    }
}
