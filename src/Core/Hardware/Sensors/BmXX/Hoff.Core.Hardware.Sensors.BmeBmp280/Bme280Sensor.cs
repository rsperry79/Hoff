using System;
using System.Device.I2c;
using System.Threading;

using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Events;
using Hoff.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Models;

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
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
        private bool validAltRead = false;
        private static II2cBussControllerService deviceScan;

        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private uint _scale = 2;
        private bool init = false;
        private Bme280 sensor;
        #endregion

        #region Properties
        private RelativeHumidity relativeHumidity;
        private Temperature temperature;
        private Pressure pressure;
        private Length altitude;

        /// <summary>
        /// Accessor/Mutator for relative humidity %
        /// </summary>
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

                    EventHandler<ITempatureChangedEventArgs> tempEvent = TemperatureChanged;
                    tempEvent(this, new TempatureChangedEventArgs(value));
                }
            }
        }

        public Pressure Pressure
        {
            get => this.pressure;
            private set
            {
                if (this.pressure.InchesOfMercury.Truncate(this._scale) != value.InchesOfMercury.Truncate(this._scale))
                {
                    this.pressure = value;

                    EventHandler<IBarometerChangedEventArgs> tempEvent = PressureChanged;
                    tempEvent(this, new BarometerChangedEventArgs(value));
                }
            }
        }

        public Length Altitude
        {
            get => this.altitude;
            private set
            {
                if (this.altitude.Feet.Truncate(this._scale) != value.Feet.Truncate(this._scale))
                {
                    this.altitude = value;

                    EventHandler<IAltimeterChangedEventArgs> tempEvent = AltimeterChanged;
                    tempEvent(this, new AltimeterChangedEventArgs(value));
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<ITempatureChangedEventArgs> TemperatureChanged;
        public event EventHandler<IAltimeterChangedEventArgs> AltimeterChanged;
        public event EventHandler<IHumidityChangedEventArgs> HumidityChanged;
        public event EventHandler<IBarometerChangedEventArgs> PressureChanged;
        #endregion

        #region Constructor

        public Bme280Sensor(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        public bool DefaultInit()
        {
            const uint scale = 2;
            int bussId = 0;
            byte deviceAddr = 0;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

            try
            {
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

                this._logger.LogDebug("Bme280 Autodetect");
                this._logger.LogDebug($"Bme280 Buss ID: {bussId}");
                this._logger.LogDebug($"Bme280 Device Address: {deviceAddr:X}");
                this._logger.LogDebug($"Bme280 Device Speed: {speed}");
                this._logger.LogDebug($"Bme280 Device Scale: {scale}");

                return this.Init(bussId, deviceAddr, speed, scale);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale)
        {
            try
            {
                if (this.sensor != null)
                {
                    this._logger.LogDebug($"ReadStatus Measuring: {this.sensor.ReadStatus().Measuring}");
                }

                if (this.sensor == null)
                {
                    this.sensor = new Bme280(I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed)));

                    if (busSpeed == I2cBusSpeed.FastMode)
                    {
                        this.sensor.TemperatureSampling = Sampling.UltraHighResolution;
                        this.sensor.PressureSampling = Sampling.UltraHighResolution;
                        this.sensor.HumiditySampling = Sampling.UltraHighResolution;
                        this.sensor.FilterMode = Bmx280FilteringMode.X4;
                    }
                    else
                    {
                        this.sensor.TemperatureSampling = Sampling.Standard;
                        this.sensor.PressureSampling = Sampling.Standard;
                        this.sensor.HumiditySampling = Sampling.Standard;
                        this.sensor.FilterMode = Bmx280FilteringMode.X2;
                    }

                    this._scale = scale;
                    this.init = true;

                    this.sensor.TryReadAltitude(out this.altitude);
                    this.sensor.TryReadPressure(out this.pressure);
                    this.sensor.TryReadHumidity(out this.relativeHumidity);
                    this.sensor.TryReadTemperature(out this.temperature);

                    this._logger.LogDebug("Bme280 Sensor Init Complete");
                }

                return this.init;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);

                this._logger.LogError(ex.StackTrace);
                throw;
            }
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
                    this.Pressure = this.ReadBarometer();
                    this.Temperature = this.ReadTemperature();
                    this.Humidity = this.ReadHumidity();
                    this.Altitude = this.ReadAltimeter();
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.StackTrace);
                    throw;
                }
            }
        }
        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            this.sensor.Dispose();
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
            try
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
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in Celsius</returns>
        protected Temperature ReadTemperature()
        {
            try
            {
                Bme280ReadResult readResult;

                do
                {
                    readResult = this.sensor.Read();
                    Thread.Sleep(sensorSleepTime);
                } while (!readResult.HumidityIsValid);

                return readResult.Temperature;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in Celsius</returns>
        protected Pressure ReadBarometer()
        {
            try
            {
                Bme280ReadResult readResult;

                do
                {
                    readResult = this.sensor.Read();
                    Thread.Sleep(sensorSleepTime);
                }
                while (!readResult.HumidityIsValid);

                return readResult.Pressure;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in Celsius</returns>
        protected Length ReadAltimeter()
        {
            try
            {
                Length altValue = Length.FromMeters(0);

                do
                {
                    this.validAltRead = this.sensor.TryReadAltitude(out altValue);

                    if (this.validAltRead)
                    {
                        return altValue;
                    }

                    Thread.Sleep(sensorSleepTime);
                }
                while (!this.validAltRead);

                return altValue;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion
    }
}
