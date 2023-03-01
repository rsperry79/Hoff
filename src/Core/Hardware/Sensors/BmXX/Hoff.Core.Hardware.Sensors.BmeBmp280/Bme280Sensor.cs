using System;
using System.Device.I2c;
using System.Threading;

using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Common.Interfaces.Services;

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using UnitsNet;

namespace Hoff.Core.Hardware.Sensors.BmXX
{

    public class Bme280Sensor : SensorBase, IBme280Sensor, IBarometer, IAltimeter, IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Implementation

        private const int sensorSleepTime = 10;
        private readonly ILogger _logger;
        private static II2cBussControllerService deviceScan;

        /// <summary>
        /// The underlying I2C device
        /// </summary>
        private I2cDevice i2CDevice = null;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private uint _scale = 2;


        private bool init;
        private Bme280 sensor;

        #endregion


        #region Properties
        /// <summary>
        /// Accessor/Mutator for relative humidity %
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
                    _ = HumiditySensorChanged();
                }
            }
        }

        private Temperature temperature;

        /// <summary>
        /// Accessors/Mutator for temperature in Celsius
        /// </summary>

        public Temperature Temperature
        {
            get => this.temperature;
            private set
            {
                if (this.temperature.DegreesCelsius.Truncate(this._scale) != value.DegreesCelsius.Truncate(this._scale))
                {
                    this.temperature = value;
                    TemperatureSensorChanged();
                }

            }
        }

        private Pressure pressure;

        public Pressure Pressure
        {
            get => this.pressure;
            private set
            {

                if (this.pressure.InchesOfMercury.Truncate(this._scale) != value.InchesOfMercury.Truncate(this._scale))
                {
                    this.pressure = value;
                    PressureSensorChanged();
                }

            }
        }

        private Length altitude;

        public Length Altitude
        {
            get => this.altitude;
            private set
            {
                if (this.altitude.Feet.Truncate(this._scale) != value.Feet.Truncate(this._scale))
                {
                    this.altitude = value;
                    AltimeterSensorChanged();
                }

            }
        }
        #endregion

        #region Events
        public event ITempatureSensor.TempatureChangedEventHandler TemperatureSensorChanged;
        public event IHumiditySensor.HumidityChangedEventHandler HumiditySensorChanged;
        public event IBarometer.PressureChangedEventHandler PressureSensorChanged;
        public event IAltimeter.AltimeterChangedEventHandler AltimeterSensorChanged;

        #endregion

        #region Constructor

        public Bme280Sensor(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        public bool DefaultInit()
        {
            int bussId = 0;
            byte deviceAddr = 0;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

            if (deviceScan.I2C1.Contains(Bme280.DefaultI2cAddress))
            {
                bussId = 1;
                deviceAddr = Bme280.DefaultI2cAddress;
                speed = deviceScan.I2C1BusSpeed;
            }

            if (deviceScan.I2C1.Contains(Bme280.SecondaryI2cAddress))
            {
                bussId = 1;
                deviceAddr = Bme280.SecondaryI2cAddress;
                speed = deviceScan.I2C1BusSpeed;
            }

            if (deviceScan.I2C2.Contains(Bme280.DefaultI2cAddress))
            {
                bussId = 2;
                deviceAddr = Bme280.DefaultI2cAddress;
                speed = deviceScan.I2C2BusSpeed;
            }

            if (deviceScan.I2C2.Contains(Bme280.SecondaryI2cAddress))
            {
                bussId = 2;
                deviceAddr = Bme280.SecondaryI2cAddress;
                speed = deviceScan.I2C2BusSpeed;
            }

            this._logger.LogDebug($"Bme280 Autodetect");
            this._logger.LogDebug($"Bme280 Buss ID: {bussId}");
            this._logger.LogDebug($"Bme280 Device Address: {deviceAddr}");

            bool complete = this.Init(bussId, deviceAddr, speed, 2);
            return complete;
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale)
        {
            if (!this.init)
            {
                this.i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed));
                this.sensor = new Bme280(this.i2CDevice);

                if (busSpeed == I2cBusSpeed.FastMode)
                {
                    this.sensor.TemperatureSampling = Sampling.LowPower;
                    this.sensor.PressureSampling = Sampling.UltraHighResolution;
                }


                this._scale = scale;

                this.init = true;

            }

            this.ReadAllSenors();
            return this.init;

        }
        #endregion

        #region Core Methods


        /// <summary>
        /// Reset the sensor...this performs a soft reset. To perform a hard reset, the system must be
        /// power cycled
        /// </summary>
        public void Reset()
        {
            this.sensor.Reset();
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
        protected override void HasSensorValueChanged()
        {
            this.ReadAllSenors();

        }

        private void ReadAllSenors()
        {
            try
            {
                this.Altitude = this.ReadAltimeter();
                this.Pressure = this.ReadBarometer();
                this.Temperature = this.ReadTemperature();
                this.Humidity = this.ReadHumidity();

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            this.i2CDevice.Dispose();
            this.i2CDevice = null;
        }

        public void Dispose()
        {
            if (!this._disposed)
            {
                this.DisposeSensor();
                this._disposed = true;
            }
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Read humidity value from the sensor
        /// </summary>
        /// <returns>humidity as a floating point number</returns>
        protected RelativeHumidity ReadHumidity()
        {

            Bme280ReadResult readResult;

            do
            {
                readResult = this.sensor.Read();
                Thread.Sleep(sensorSleepTime);
            }
            while (!readResult.HumidityIsValid);

            return readResult.Humidity;

        }

        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in celcius</returns>
        protected Temperature ReadTemperature()
        {

            Bme280ReadResult readResult;

            do
            {
                readResult = this.sensor.Read();
                Thread.Sleep(sensorSleepTime);
            }
            while (!readResult.TemperatureIsValid);

            return readResult.Temperature;
        }


        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in celcius</returns>
        protected Pressure ReadBarometer()
        {

            Bme280ReadResult readResult;

            do
            {
                readResult = this.sensor.Read();
                Thread.Sleep(sensorSleepTime);
            }
            while (!readResult.PressureIsValid);

            return readResult.Pressure;
        }


        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in celcius</returns>
        protected Length ReadAltimeter()
        {
            Length altValue;
            bool validRead;

            do
            {
                validRead = this.sensor.TryReadAltitude(out altValue);
                Thread.Sleep(sensorSleepTime);
            }
            while (!validRead);


            return altValue;
        }
        #endregion
    }
}
