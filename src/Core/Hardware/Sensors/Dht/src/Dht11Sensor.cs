using System;
using System.Diagnostics;

using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;

using Iot.Device.DHTxx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Hardware.Sensors.Dht
{
    public class Dht11Sensor : SensorBase, IDht11Sensor, IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
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
        private double humidity = 1;

        public double Humidity
        {
            get
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return this.humidity;
                }
                else
                {
                    return -99999;
                }


            }
            set
            {

                if (Dht.IsLastReadSuccessful && this.humidity != value)
                {
                    this.logger.LogTrace($"Set humidity: {value}");

                    this.humidity = value;
                    _ = HumiditySensorChanged();
                }
            }
        }


        private double temperature = -9999;

        private bool disposedValue;

        /// <summary>
        /// Accessor/Mutator for temperature in Celsius
        /// </summary>

        public double Temperature
        {
            get
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return this.temperature;
                }
                else
                {
                    return -99999;
                }
            }
            set
            {
                if (Dht.IsLastReadSuccessful && this.temperature != value)
                {
                    this.logger.LogTrace($"Set Temp: {value}");
                    this.temperature = value;
                    TemperatureSensorChanged();
                }

            }
        }


        #endregion

        #region Event Handlers
        /// <summary>
        /// Temperature Changed Event handler
        /// </summary>
        public event ITempatureSensor.TempatureChangedEventHandler TemperatureSensorChanged;

        /// <summary>
        /// Humidity changed event handler.
        /// </summary>
        public event IHumiditySensor.HumidityChangedEventHandler HumiditySensorChanged;
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

        /// <summary>
        /// Let the world know whether the sensor value has changed or not
        /// </summary>
        /// <returns>bool</returns>
        public override void HasSensorValueChanged()
        {
            try
            {

                float temp = ((float)this.ReadTemperature()).Truncate(this._scale);
                this.Temperature = temp;
                float hum = ((float)this.ReadHumidity()).Truncate(this._scale);
                this.Humidity = hum;
            }
            catch (Exception)
            {
                Debug.WriteLine("HasSensorValueChanged");

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
        private double ReadHumidity()
        {
            try
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return Dht.Humidity.Percent;
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("ReadHumidity");
            }

            return default;
        }

        private double ReadTemperature()
        {
            try
            {
                if (Dht.IsLastReadSuccessful)
                {
                    return Dht.Temperature.DegreesCelsius;
                }
                else
                {

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
