using System;
using System.Device.I2c;
using System.Diagnostics;

using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Base;

using Iot.Device.DHTxx;

using UnitsNet;

namespace Hoff.Hardware.Sensors.Environmental
{
    public class Dht12Sensor : SensorBase, IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Implementation
        /// <summary>
        /// The underlying I2C device
        /// </summary>
        private I2cDevice _i2CDevice = null;

        private readonly Dht12 Dht = null;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private readonly uint _scale = 2;
        #endregion

        #region Properties
        /// <summary>
        /// Accessors/Mutator for relative humidity %
        /// </summary>
        private double humidity = 1;

        public RelativeHumidity Humidity
        {
            get
            {
                Debug.WriteLine(this.humidity.ToString());
                return RelativeHumidity.FromPercent(this.humidity);
            }
            set
            {

                if (this.humidity != value.Percent)
                {
                    this.humidity = value.Percent;
                    HumiditySensorChanged();
                }
            }
        }


        private Temperature temperature = Temperature.FromDegreesCelsius(-9999);

        private bool disposedValue;

        /// <summary>
        /// Accessor/Mutator for temperature in celcius
        /// </summary>

        public Temperature Temperature
        {
            get => this.temperature;
            set
            {
                if (this.temperature.DegreesCelsius != value.DegreesCelsius)
                {
                    this.temperature = value;
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

        /// <summary>
        /// Humidity changed event handler.
        /// </summary>
        public event IHumiditySensor.HumidityChangedEventHandler HumiditySensorChanged;
        #endregion

        #region Constructor

        public Dht12Sensor(int busSelector = 1, byte deviceAddr = Dht12.DefaultI2cAddress, I2cBusSpeed speed = I2cBusSpeed.FastMode,
        uint scale = 2)
        {
            try
            {


                I2cConnectionSettings settings = new(busSelector, deviceAddr, speed);
                this._i2CDevice = I2cDevice.Create(settings);
                this.Dht = new(this._i2CDevice);

                this._scale = scale;
            }
            catch (Exception)
            {
                Debug.WriteLine("CTOR");
                throw;
            }
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
            try
            {

                float temp = ((float)this.ReadTemperature().DegreesCelsius).Truncate(this._scale);
                this.Temperature = Temperature.FromDegreesCelsius(temp);
                float hum = ((float)this.ReadHumidity().Percent).Truncate(this._scale);
                this.Humidity = RelativeHumidity.FromPercent(hum);
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
            this._i2CDevice.Dispose();
            this._i2CDevice = null;
        }



        ~Dht12Sensor()
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
        private UnitsNet.RelativeHumidity ReadHumidity()
        {
            try
            {
                return this.Dht.Humidity;
            }
            catch (Exception)
            {
                Debug.WriteLine("ReadHumidty");
            }

            return default;
        }

        private Temperature ReadTemperature()
        {
            return this.Dht.Temperature;
        }




        #endregion

    }
}
